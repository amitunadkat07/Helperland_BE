using Helperland.Entity.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Helperland.Repository.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration Configuration;
        public TokenService(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        public string GenerateJWTAuthetication(UserDataModel userData)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userData.Email),
                new Claim(ClaimTypes.Role, userData.RoleId.ToString()),
                new Claim("Name", userData.FirstName + " " + userData.LastName ),
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires =
                DateTime.UtcNow.AddSeconds(30);

            var token = new JwtSecurityToken(
                Configuration["Jwt:Issuer"],
                Configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
