using Core.Attributes;
using Core.Context;
using Core.Models;
using Interfaces.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UserMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {

        private readonly CookingContext _context;

        public UserController(CookingContext context)
        {
            _context = context;
        }

        //Add Cookies Auth Attribute
        [HttpGet()]
        [CustomTokenAuthentication("Owner, Admin")]
        public async Task<IEnumerable<IUser>> GetUsers()
        {
            var list = new List<IUser>();
            foreach (var item in await _context.Users.ToListAsync())
            {
                list.Add(new User(item));
            }

            return list;
        }

        [HttpGet("{id}")]
        [CustomTokenAuthentication("Owner, Admin")]
        public async Task<IUser> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return user != null ? new User(user) : null;
        }
    }
}
