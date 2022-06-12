using IDS.Interfaces;
using Interfaces.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IDS.Tokens
{
    public class JwtGenerator : IJwtGenerator
    {
		private readonly SymmetricSecurityKey _key;

		//TODO: Hold key in secret proj holder
		private readonly string _secret = "IDS_WebAPI_Secret_Key";

		public JwtGenerator(IConfiguration config)
		{
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
		}

		public string CreateToken(IUserIDS user, bool rememberMe)
		{
			var claims = new[] { 
				new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
			};

			var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = rememberMe ? DateTime.Now.AddDays(7) : DateTime.Now.AddHours(1),
				SigningCredentials = credentials
			};
			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}
