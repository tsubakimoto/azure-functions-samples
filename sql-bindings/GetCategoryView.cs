using System.Collections.Generic;
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

namespace AzureSqlBindingsSample
{
    public class GetCategoryView
    {
        private readonly ILogger<GetCategoryView> _logger;

        public GetCategoryView(ILogger<GetCategoryView> log)
        {
            _logger = log;
        }

        [FunctionName("GetCategoryView")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Sql(
                commandText: "select * from SalesLT.vGetAllCategories where ParentProductCategoryName = @Name",
                connectionStringSetting: "SqlConnectionString",
                commandType: System.Data.CommandType.Text,
                parameters: "@Name={Query.name}")] IEnumerable<CategoryView> categories)
        {
            _logger.LogInformation("GetCategoryView is running.");
            return new OkObjectResult(categories);
        }
    }
}

