using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace cosmos_change_feed;

public class Feed
{
    private readonly ILogger _logger;

    public Feed(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Feed>();
    }

    [Function("Function1")]
    public void Run([CosmosDBTrigger(
        databaseName: "ToDoList",
        containerName: "Items",
        Connection = "CosmosDbConnectionString",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<TodoItem> input)
    {
        if (input != null && input.Count > 0)
        {
            _logger.LogInformation("Todos modified: " + input.Count);
            _logger.LogInformation("First todo Id: " + input[0].id);
            _logger.LogInformation("First todo Description: " + input[0].description);
        }
    }
}

public class TodoItem
{
    public string id { get; set; }

    public string description { get; set; }
}

/* JSON schema
{
    "id": "1",
    "description": "title1"
}
*/
