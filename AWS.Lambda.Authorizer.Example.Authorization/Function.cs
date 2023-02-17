using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using AWS.Lambda.Authorizer.Example.Authorization.Services;
using AWS.Lambda.Authorizer.Example.Authorization.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWS.Lambda.Authorizer.Example.Authorization;

public class Function
{
    readonly IServiceProvider _serviceProvider;
    public Function()
    {
        ServiceCollection serviceCollection = new();
        serviceCollection.AddScoped<ITokenValidator, TokenValidator>();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
    public APIGatewayCustomAuthorizerResponse TokenValidator(APIGatewayCustomAuthorizerRequest request, ILambdaContext context)
    {
        string accessToken = request.Headers["authorization"];
        ITokenValidator tokenValidator = _serviceProvider.GetRequiredService<ITokenValidator>();
        ClaimsPrincipal claimsPrincipal = tokenValidator.ValidateToken(accessToken);

        string? principalId = "401";
        if (claimsPrincipal is { Identity.IsAuthenticated: true })
            principalId = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;

        return new APIGatewayCustomAuthorizerResponse()
        {
            PrincipalID = principalId,
            PolicyDocument = new APIGatewayCustomAuthorizerPolicy
            {
                Statement = new List<APIGatewayCustomAuthorizerPolicy.IAMPolicyStatement>
                    {
                         new APIGatewayCustomAuthorizerPolicy.IAMPolicyStatement()
                         {
                             Effect = claimsPrincipal != null ? "Allow" : "Deny",
                             Resource = new HashSet<string>{ "arn:aws:execute-api:ap-south-1:567921601443:uyv8v6df12/*/*" },
                             Action = new HashSet<string>{ "execute-api:Invoke" }
                         }
                    }
            }
        };
    }
}
