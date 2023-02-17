using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AWS.Lambda.Authorizer.Example.Authentication.Models
{
    [DynamoDBTable("users")]
    public class User
    {
        [DynamoDBHashKey("email"), DynamoDBProperty("email")]
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [DynamoDBProperty("username")]
        [JsonPropertyName("username")]
        public string? Username { get; set; }
        [DynamoDBProperty("password")]
        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}
