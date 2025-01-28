using BusProyectApi.Models.Entities;
using BusProyectApi.Controllers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusProyectApi.Controllers
{
    public class TokenService
    {
        private const string SecretKey = "MirandaLawsonIsTheBestCharacterInMassEffect"; // Make sure this matches the key in Program.cs
        private const string Issuer = "Cerberus";
        private const string Audience = "Omega";

        public static string GenerateToken(int userId, bool isAdmin)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, isAdmin ? "Admin" : "User"),
                new Claim("IsAdmin", isAdmin.ToString()) // Add IsAdmin as a claim
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
