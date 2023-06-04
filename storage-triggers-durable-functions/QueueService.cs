using Azure.Storage.Queues;

namespace StorageTriggersDurableFunctions;

public class QueueService : IQueueService
{
    private readonly QueueServiceClient queueServiceClient;
    private readonly QueueClient queueClient;

    public QueueService(
        QueueServiceClient queueServiceClient,
        QueueClient queueClient)
    {
        this.queueServiceClient = queueServiceClient;
        this.queueClient = queueClient;
    }

    public QueueServiceClient GetQueueServiceClient() => queueServiceClient;

    public QueueClient GetQueueClient() => queueClient;
}

public interface IQueueService
{
    QueueServiceClient GetQueueServiceClient();
    QueueClient GetQueueClient();
}