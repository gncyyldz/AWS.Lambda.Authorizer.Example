using Amazon.Lambda.Core;
using AWS.Lambda.Authorizer.Example.Authorization.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AWS.Lambda.Authorizer.Example.Authorization.Services
{
    public class TokenValidator : ITokenValidator
    {
        public ClaimsPrincipal ValidateToken(string accessToken)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            byte[] secretKey = Encoding.UTF8.GetBytes("doldur be meyhaneci, boş kalmasın kadehim...");

            TokenValidationParameters validationParameters = new()
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            };


            try
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(
                     token: accessToken,
                     validationParameters: validationParameters,
                     validatedToken: out SecurityToken _);
                return claimsPrincipal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}