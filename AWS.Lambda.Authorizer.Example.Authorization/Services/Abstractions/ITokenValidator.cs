using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Lambda.Authorizer.Example.Authorization.Services.Abstractions
{
    public interface ITokenValidator
    {
        ClaimsPrincipal ValidateToken(string accessToken);
    }
}
