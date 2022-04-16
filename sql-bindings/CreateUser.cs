using System.IO;
using System.Net;
using AzureSqlBindingsSample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace AzureSqlBindingsSample
{
    public class CreateUser
    {
        private readonly ILogger<CreateUser> _logger;

        public CreateUser(ILogger<CreateUser> log)
        {
            _logger = log;
        }

        [FunctionName("CreateUser")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "The request body")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(User), Description = "The created response")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Sql("dbo.User",
                ConnectionStringSetting = "SqlConnectionString")] out User newUser)
        {
            _logger.LogInformation("CreateUser is running.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            User user = JsonConvert.DeserializeObject<User>(requestBody);
            newUser = new User
            {
                Name = user?.Name,
                Age = user?.Age ?? 1,
            };

            return new CreatedResult("/api/users", newUser);
        }

        [FunctionName("CreateUser2")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "The request body")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(User), Description = "The created response")]
        public IActionResult Run2(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Sql("dbo.User",
                ConnectionStringSetting = "SqlConnectionString")] out User[] output)
        {
            _logger.LogInformation("CreateUser is running.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            User user = JsonConvert.DeserializeObject<User>(requestBody);

            output = new User[1];
            output[0] = new User
            {
                Name = user?.Name,
                Age = user?.Age ?? 1,
            };

            return new CreatedResult("/api/users", output);
        }
    }
}

