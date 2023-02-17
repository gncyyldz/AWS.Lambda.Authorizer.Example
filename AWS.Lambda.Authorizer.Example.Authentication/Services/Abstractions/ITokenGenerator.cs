using AWS.Lambda.Authorizer.Example.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Lambda.Authorizer.Example.Authentication.Services.Abstractions
{
    public interface ITokenGenerator
    {
        string GenerateJWT(User user, int minute);
    }
}
