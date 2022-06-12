using IDS.DB;
using IDS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IDS.Tokens
{
    public class TokenManager : ITokenManager
    {

        private readonly IDS_Context _context;

        public TokenManager(IDS_Context context)
        {
            _context = context;
        }

        public async Task<bool> VerifyToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var userToken = await _context.Tokens.FirstOrDefaultAsync(u => u.Token.Equals(token));

            return DateTime.Now < userToken.ExpairedAt;
        }

        public async Task<string> GetUserRoles(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return string.Empty;
            }

            var userToken = await _context.Tokens.FirstOrDefaultAsync(u => u.Token.Equals(token));

            var userDB = await _context.Users.FirstOrDefaultAsync(u => u.Id == userToken.UserId);

            if (userDB == null || DateTime.Now >= userToken.ExpairedAt)
            {
                return string.Empty;

            }

            return userDB.Roles;
        }
    }
}
