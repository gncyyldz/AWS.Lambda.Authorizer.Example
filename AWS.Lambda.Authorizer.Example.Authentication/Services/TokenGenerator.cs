using AWS.Lambda.Authorizer.Example.Authentication.Models;
using AWS.Lambda.Authorizer.Example.Authentication.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Lambda.Authorizer.Example.Authentication.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        public string GenerateJWT(User user, int minute)
        {
            List<Claim> claims = new() {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name,user.Username)
            };

            byte[] secretKey = Encoding.UTF8.GetBytes("doldur be meyhaneci, boş kalmasın kadehim...");

            SigningCredentials credentials = new(
                key: new SymmetricSecurityKey(secretKey),
                algorithm: SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minute),
                signingCredentials: credentials);

            JwtSecurityTokenHandler tokenHandler = new();
            return tokenHandler.WriteToken(token);
        }
    }
}
