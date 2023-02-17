using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using AWS.Lambda.Authorizer.Example.Models;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWS.Lambda.Authorizer.Example;

public class Function
{
    public async Task<APIGatewayHttpApiV2ProxyResponse> CreatePersonAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        Person? person = JsonSerializer.Deserialize<Person>(request.Body);
        AmazonDynamoDBClient dynamoDBClient = new();
        DynamoDBContext dynamoDbContext = new(dynamoDBClient);
        await dynamoDbContext.SaveAsync(person);
        string message = $"Person created. Name : {person.Name}";
        LambdaLogger.Log(message);
        return new()
        {
            Body = message,
            StatusCode = 200
        };
    }
    public async Task<List<Person>> GetAllPersonsAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        AmazonDynamoDBClient dynamoDBClient = new();
        DynamoDBContext dynamoDBContext = new(dynamoDBClient);
        List<Person> persons = await dynamoDBContext.ScanAsync<Person>(default).GetRemainingAsync();
        return persons;
    }
    public async Task<Person> GetPersonsByIdAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        AmazonDynamoDBClient dynamoDBClient = new();
        DynamoDBContext dynamoDBContext = new(dynamoDBClient);
        int personId = int.Parse(request.PathParameters["id"]);
        Person person = await dynamoDBContext.LoadAsync<Person>(personId);
        if (person == null)
            throw new Exception("Person not found!");
        return person;
    }
}
