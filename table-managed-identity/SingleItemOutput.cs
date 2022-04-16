using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AzureTableManagedIdentitySample
{
    public static class SingleItemOutput
    {
        [FunctionName("SingleItemOutput")]
        [return: Table("Items", Connection = "MyTableService")]
        public static TableEntity Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string key = req.Query["key"];
            return new TableEntity("item1", key);
        }
    }
}
