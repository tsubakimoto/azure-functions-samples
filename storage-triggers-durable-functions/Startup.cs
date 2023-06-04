using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(DurableFunctionApp1.Startup))]
namespace DurableFunctionApp1;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();

        builder.Services.AddAzureClients(clientBuilder =>
        {
            // https://stackoverflow.com/a/74421494
            clientBuilder.AddClient<QueueClient, QueueClientOptions>(options =>
            {
                IServiceProvider sp = builder.Services.BuildServiceProvider();
                IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
                string connectionString = configuration["AzureWebJobsStorage"];
                return new(connectionString, "myqueue-items");
            });

            IServiceProvider sp = builder.Services.BuildServiceProvider();
            IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
            string connectionString = configuration["AzureWebJobsStorage"];

            // https://learn.microsoft.com/ja-jp/dotnet/azure/sdk/dependency-injection
            clientBuilder.AddQueueServiceClient(connectionString);
            clientBuilder.AddBlobServiceClient(connectionString);
        });

        builder.Services.AddSingleton<IQueueService, QueueService>();
    }
}
