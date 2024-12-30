using System.ClientModel;
using Azure.AI.OpenAI;
using Microsoft.Azure.Functions.Worker.Builder;
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

builder.Build().Run();
