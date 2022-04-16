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
    public static class CloudTableInput
    {
        [FunctionName("CloudTableInput")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Table("Items", Connection = "MyTableService")] TableClient tableClient,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var rowKeys = new List<string>();

            AsyncPageable<TableEntity> queryResults = tableClient.QueryAsync<TableEntity>(filter: "PartitionKey eq 'item1'");
            await foreach (TableEntity entity in queryResults)
            {
                log.LogInformation($"{entity.PartitionKey}\t{entity.RowKey}\t{entity.Timestamp}");
                rowKeys.Add(entity.RowKey);
            }

            return new OkObjectResult(rowKeys);
        }
    }
}
