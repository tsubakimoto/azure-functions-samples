using System;
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
    public class CreateProductModel
    {
        private readonly ILogger<CreateProductModel> _logger;

        public CreateProductModel(ILogger<CreateProductModel> log)
        {
            _logger = log;
        }

        [FunctionName("CreateProductModel")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ProductModel), Required = true, Description = "The request body")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(ProductModel), Description = "The created response")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Sql("SalesLT.ProductModel",
                ConnectionStringSetting = "SqlConnectionString")] out ProductModel newProductModel)
        {
            _logger.LogInformation("CreateProductModel is running.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            ProductModel productModel = JsonConvert.DeserializeObject<ProductModel>(requestBody);
            newProductModel = new ProductModel
            {
                Name = productModel?.Name,
                ModifiedDate = DateTime.UtcNow
            };

            return new CreatedResult("/api/productModels", newProductModel);
        }
    }
}
