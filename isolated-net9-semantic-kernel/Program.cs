using System.ClientModel;
using Azure.AI.OpenAI;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

var configuration = builder.Configuration;

builder.Services.AddAzureOpenAIChatCompletion(
    configuration["AzureOpenAI:DeploymentName"],
    new AzureOpenAIClient(
        new Uri(configuration["AzureOpenAI:Endpoint"]),
        new ApiKeyCredential(configuration["AzureOpenAI:ApiKey"])));

// Add the Semantic Kernel services to the service collection.
builder.Services.AddKernel();

// Register the Demographics plugin with the Kernel.
builder.Services.AddTransient(sp =>
{
    KernelPluginCollection plugins = [];
    plugins.AddFromType<Demographics>();
    return plugins;
});

builder.Build().Run();

class Demographics
{
    [KernelFunction]
    public int GetPersonAge(string name)
    {
        return name switch
        {
            "John" => 45,
            "Smith" => 37,
            _ => 40
        };
    }
}
