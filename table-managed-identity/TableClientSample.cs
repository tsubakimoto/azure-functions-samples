using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AzureTableManagedIdentitySample
{
    public static class TableClientSample
    {
        [FunctionName("TableClientInput")]
        public static async Task<IActionResult> TableClientInputRun(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("Items", Connection = "MyTableService")] TableClient tableClient,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var rowKeys = new List<string>();

            AsyncPageable<TableEntity> queryResults = tableClient.QueryAsync<TableEntity>(filter: "PartitionKey eq 'item2'");
            await foreach (TableEntity entity in queryResults)
            {
                log.LogInformation($"{entity.PartitionKey}\t{entity.RowKey}\t{entity.Timestamp}");
                rowKeys.Add(entity.RowKey);
            }

            return new OkObjectResult(rowKeys);
        }

        [FunctionName("TableClientOutput")]
        public static async Task<IActionResult> TableClientOutputRun(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("Items", Connection = "MyTableService")] TableClient tableClient,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string key = req.Query["key"];
            await tableClient.AddEntityAsync(new TableEntity("item2", key));
            return new NoContentResult();
        }
    }
}
