// https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-azure-sql-output?tabs=csharp

using System;
using System.Net;
using System.Threading.Tasks;
using AzureSqlBindingsSample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace AzureSqlBindingsSample
{
    public static class WriteOneRecord
    {
        [FunctionName("WriteOneRecord")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(ToDoItem), Description = "The created response")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "addtodo")] HttpRequest req,
            ILogger log,
            [Sql(
                commandText: "dbo.ToDo",
                connectionStringSetting: "SqlConnectionString")] out ToDoItem newItem)
        {
            newItem = new ToDoItem
            {
                Id = req.Query["id"],
                Description = req.Query["desc"]
            };

            log.LogInformation($"C# HTTP trigger function inserted one row");
            return new CreatedResult($"/api/addtodo", newItem);
        }

        [FunctionName("WriteRecordsAsync")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "text/plain", bodyType: typeof(string), Description = "The created response")]
        public static async Task<IActionResult> Run2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "addtodo-asynccollector")] HttpRequest req,
            ILogger log,
            [Sql(
                commandText: "dbo.ToDo",
                connectionStringSetting: "SqlConnectionString")] IAsyncCollector<ToDoItem> newItems)
        {
            await newItems.AddAsync(new ToDoItem
            {
                Id = DateTime.UtcNow.Millisecond.ToString(),
                Description = DateTime.UtcNow.ToString()
            });
            await newItems.AddAsync(new ToDoItem
            {
                Id = (DateTime.UtcNow.Millisecond + 100).ToString(),
                Description = DateTime.UtcNow.AddDays(1).ToString()
            });
            // Rows are upserted here
            await newItems.FlushAsync();

            return new CreatedResult($"/api/addtodo-asynccollector", "done");
        }

        [FunctionName("WriteRecordsSync")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "text/plain", bodyType: typeof(string), Description = "The created response")]
        public static IActionResult Run3(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "addtodo-ccollector")] HttpRequest req,
            ILogger log,
            [Sql(
                commandText: "dbo.ToDo",
                connectionStringSetting: "SqlConnectionString")] ICollector<ToDoItem> newItems)
        {
            newItems.Add(new ToDoItem
            {
                Id = DateTime.UtcNow.Millisecond.ToString(),
                Description = DateTime.UtcNow.ToString()
            });
            newItems.Add(new ToDoItem
            {
                Id = (DateTime.UtcNow.Millisecond + 100).ToString(),
                Description = DateTime.UtcNow.AddDays(1).ToString()
            });

            return new CreatedResult($"/api/addtodo-collector", "done");
        }
    }
}
