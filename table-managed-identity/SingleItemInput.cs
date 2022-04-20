using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AzureTableManagedIdentitySample
{
    public static class SingleItemInput
    {
        [FunctionName("SingleItemInput")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Table("Items", "item1", "1", Connection = "MyTableService")] TableEntity entity,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string responseMessage = $"PK={entity.PartitionKey}, RK={entity.RowKey}, Timestamp={entity.Timestamp}";
            return new OkObjectResult(responseMessage);
        }
    }
}
