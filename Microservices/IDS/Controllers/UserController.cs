using Core.Models;
using Core.RabbitMQ.Models;
using IDS.Attributes;
using IDS.DB;
using IDS.DB.Models;
using IDS.Extensions;
using IDS.Interfaces;
using IDS.Models;
using Interfaces.Models;
using Interfaces.RabbitMQ;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IDS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {

        private readonly IDS_Context _context;

        private readonly IJwtGenerator _jwtGenerator;

        private readonly IDataProtectionProvider _provider;

        private readonly IRabbitBus _bus;

        public UserController(IDS_Context context, 
                              IJwtGenerator jwtGenerator,
                              IDataProtectionProvider provider, 
                              IRabbitBus bus)
        {
            _context = context;
            _jwtGenerator = jwtGenerator;
            _provider = provider;
            _bus = bus;
        }

        [CustomTokenAuthentication("Owner")]
        [HttpGet()]
        public async Task<IEnumerable<IUserIDS>> GetUsers()
        {
           return await _context.Users.ToListAsync();
        }

        [CustomTokenAuthentication("Owner")]
        [HttpGet("{userName}")]
        public async Task<IUserIDS> GetUser(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        [HttpPost("GetUserByToken")]
        public async Task<IUserIDS> GetUserByToken(TokenUsernameModel tokenUsername)
        {
            if (string.IsNullOrEmpty(tokenUsername.Token))
            {
                return new UserIDS();
            }

            var userToken = await _context.Tokens.FirstOrDefaultAsync(u => u.Token.Equals(tokenUsername.Token));

            if(userToken == null || userToken.UserId < 0)
            {
                return new UserIDS();
            }

            var userDB = await _context.Users.FirstOrDefaultAsync(u => u.Id == userToken.UserId);
            return userDB;
        }

        [HttpPost("GetUserRoles")]
        public async Task<string> GetUserRoles(TokenUsernameModel tokenUsername)
        {
            if (string.IsNullOrEmpty(tokenUsername.Token))
            {
                return string.Empty;
            }

            var userToken = await _context.Tokens.FirstOrDefaultAsync(u => u.Token.Equals(tokenUsername.Token));
            if (userToken == null || userToken.UserId < 0)
            {
                return string.Empty;
            }

            var userDB = await _context.Users.FirstOrDefaultAsync(u => u.Id == userToken.UserId);

            return userDB.Roles;
        }

        [CustomTokenAuthentication("Owner")]
        [HttpPost("ChangeRoles")]
        public async Task<IUserIDS> ChangeRolesUser(ChangeRolesModel changeRolesModel)
        {
            var userDB = await _context.Users.FirstOrDefaultAsync(u => u.UserName == changeRolesModel.UserName);

            if (userDB == null)
            {
                return new UserIDS();
            }

            var userToAdd = new UserIDS()
            {
                UserName = userDB.UserName,
                Name = userDB.Name,
                PasswordHash = userDB.PasswordHash,
                Roles = changeRolesModel.Roles
            };


            await RemoveTokens(userDB);
            _context.Users.Remove(userDB);
            var res = await _context.Users.AddAsync(userToAdd);
            await _context.SaveChangesAsync();
            await GenerateToken(res.Entity, true);

            return res.Entity;
        }

        [HttpPost("Login")]
        public async Task<string> Login(LoginUserModel user)
        {
            var userDB = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);

            if (userDB == null)
            {
                return string.Empty;
            }

            var pass = userDB.PasswordHash.DecryptePassword(_provider);

            if (!user.Password.Equals(pass))
            {
                return string.Empty;
            }

            return await GenerateToken(userDB, user.RememberMe);
        }

        [HttpPost("Register")]
        public async Task<string> Register(RegisterUserModel user)
        {
            var userDB = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);

            if (userDB != null)
            {
                return null;
            }

            var userToAdd = new UserIDS()
            {
                UserName = user.UserName,
                Name = user.Name,
                PasswordHash = user.Password.EncryptePassword(_provider),
                //TODO: Change to const
                Roles = "User"
            };

            var res = await _context.Users.AddAsync(userToAdd);
            await _context.SaveChangesAsync();
            await _bus.SendAsync<UserAddedModel>("Users", new UserAddedModel() { Id = userToAdd.Id, Name = userToAdd.Name });
            //TODO: Add User To Users via RabbitMQ

            return await GenerateToken(res.Entity, true);
        }

        [HttpPost("Logout")]
        [CustomTokenAuthentication("Owner, Admin, User")]
        public async Task<bool> Logout(TokenUsernameModel user)
        {
            if (string.IsNullOrEmpty(user.Token))
            {
                return false;
            }

            var userToken = await _context.Tokens.FirstOrDefaultAsync(u => u.Token.Equals(user.Token));

            if (userToken == null || userToken.UserId < 0)
            {
                return false;
            }

            var userDB = await _context.Users.FirstOrDefaultAsync(u => u.Id == userToken.UserId);

            if (userDB == null)
            {
                return false;
            }

            return await RemoveTokens(userDB);
        }

        [HttpPost("VerifyToken")]
        public async Task<bool> VerifyToken(TokenUsernameModel tokenUsername)
        {
            if (string.IsNullOrEmpty(tokenUsername.Token))
            {
                return false;
            }

            var userToken = await _context.Tokens.FirstOrDefaultAsync(u => u.Token.Equals(tokenUsername.Token));

            if (userToken == null || userToken.UserId < 0)
            {
                return false;
            }
            var userDB = await _context.Users.FirstOrDefaultAsync(u => u.Id == userToken.UserId);

            return userToken != null && userDB != null;
        }

        #region Private Methods
        private async Task<string> GenerateToken(IUserIDS entity, bool rememberMe)
        {
            await RemoveTokens(entity);

            //TODO: Add Token generation
            var token = _jwtGenerator.CreateToken(entity, rememberMe);
            var expires = rememberMe ? DateTime.Now.AddDays(7) : DateTime.Now.AddHours(1);

            var userToken = new UserTokenIDS()
            {
                ExpairedAt = expires,
                Token = token,
                UserId = entity.Id
            };

            await _context.Tokens.AddAsync(userToken);
            await _context.SaveChangesAsync();

            return token;
        }

        private async Task<bool> RemoveTokens(IUserIDS entity)
        {
            var tokens = new List<UserTokenIDS>();

            await _context.Tokens.ForEachAsync(u =>
            {
                if (u.UserId == entity.Id)
                {
                    tokens.Add(u);
                }
            });

            if (!tokens.Any())
            {
                return false;
            }

            tokens.ForEach((t) =>
            {
                _context.Remove(t);
            });

            await _context.SaveChangesAsync();

            return true;
        }

        #endregion

    }
}
