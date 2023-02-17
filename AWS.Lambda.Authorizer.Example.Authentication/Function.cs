using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using AWS.Lambda.Authorizer.Example.Authentication.Models;
using AWS.Lambda.Authorizer.Example.Authentication.Services;
using AWS.Lambda.Authorizer.Example.Authentication.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWS.Lambda.Authorizer.Example.Authentication;

public class Function
{
    readonly IServiceProvider _serviceProvider;
    public Function()
    {
        ServiceCollection serviceCollection = new();
        serviceCollection.AddScoped<ITokenGenerator, TokenGenerator>();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
    public async Task<string> GenerateTokenAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        User? user = JsonSerializer.Deserialize<User>(request.Body);
        AmazonDynamoDBClient dynamoDBClient = new();
        DynamoDBContext dynamoDBContext = new(dynamoDBClient);

        User? hasUser = await dynamoDBContext.LoadAsync<User>(user?.Email);
        if (hasUser != null)
        {
            if (hasUser.Password != user.Password)
                throw new Exception("Invalid credentials!");

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                ITokenGenerator tokenGenerator = scope.ServiceProvider.GetService<ITokenGenerator>();
                return tokenGenerator.GenerateJWT(user, 3);
            }
        }
        throw new Exception("User not found!");
    }
}
