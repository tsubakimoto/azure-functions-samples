using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace StorageTriggersDurableFunctions;

public class Orchestrator
{
    private readonly ILogger<Orchestrator> logger;
    private readonly IQueueService queueService;
    private readonly BlobServiceClient blobServiceClient;

    public Orchestrator(
        ILogger<Orchestrator> logger,
        IQueueService queueService,
        BlobServiceClient blobServiceClient)
    {
        this.logger = logger;
        this.queueService = queueService;
        this.blobServiceClient = blobServiceClient;
    }

    [FunctionName("Orchestrator")]
    public async Task<List<string>> RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        var outputs = new List<string>();

        // Replace "hello" with the name of your Durable Activity Function.
        outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"));
        outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
        outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));

        // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
        return outputs;
    }

#if false
    [FunctionName(nameof(SayHello))]
    public string SayHello([ActivityTrigger] string name)
    {
        logger.LogInformation("Saying hello to {name}.", name);
        return $"Hello {name}!";
    }
#endif

    [FunctionName(nameof(SayHello))]
    public string SayHello([ActivityTrigger] IDurableActivityContext context)
    {
        var name = context.GetInput<string>();
        logger.LogInformation("Saying hello to {name}.", name);
        return $"Hello {name}!";
    }

    [FunctionName("Orchestrator_HttpStart")]
    public async Task<HttpResponseMessage> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
        [DurableClient] IDurableOrchestrationClient starter)
    {
        // Function input comes from the request content.
        string instanceId = await starter.StartNewAsync("Orchestrator", null);

        logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

        return starter.CreateCheckStatusResponse(req, instanceId);
    }

    #region Storage triggers

    [FunctionName(nameof(BlobTriggerFunction))]
    public async Task BlobTriggerFunction(
        [BlobTrigger("myblob-items/{name}")] BlobClient myBlob,
        [DurableClient] IDurableOrchestrationClient starter)
    {
        string instanceId = await starter.StartNewAsync(nameof(OrchestratorFunction), null, myBlob);

        // Get clients from BlobClient
        BlobContainerClient containerClient = myBlob.GetParentBlobContainerClient();
        BlobServiceClient blobServiceClient = containerClient.GetParentBlobServiceClient();

        // Show properties from BlobServiceClient
        logger.LogInformation($"BlobServiceClient:");
        logger.LogInformation($"\tAccontName: {blobServiceClient.AccountName}");
        logger.LogInformation($"\tUri: {blobServiceClient.Uri}");

        logger.LogInformation($"Started orchestration with ID = '{instanceId}' by blob trigger.");
    }

    [FunctionName(nameof(QueueTriggerFunction))]
    public async Task QueueTriggerFunction(
        [QueueTrigger("myqueue-items")] string myQueueItem,
        [DurableClient] IDurableOrchestrationClient starter)
    {
        string instanceId = await starter.StartNewAsync(nameof(OrchestratorFunction), null, myQueueItem);

        // Enqueue
        QueueClient queueClient = queueService.GetQueueClient();
        queueClient.SendMessage($"{myQueueItem}{myQueueItem}");

        // Create queue
        QueueServiceClient queueServiceClient = queueService.GetQueueServiceClient();
        QueueClient queueClient2 = queueServiceClient.CreateQueue("myqueue-items-2");
        queueClient2.SendMessage(myQueueItem);

        logger.LogInformation($"Started orchestration with ID = '{instanceId}' by queue trigger.");
    }

    [FunctionName(nameof(OrchestratorFunction))]
    public async Task<List<string>> OrchestratorFunction(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        //string input = context.GetInput<string>();
        var outputs = new List<string>();

        // Replace "hello" with the name of your Durable Activity Function.
        outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"));
        outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
        outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));
        //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), input));

        return outputs;
    }

    #endregion

}
