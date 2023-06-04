using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace InstanceCounter;

public class Function1
{
    private readonly ILogger _logger;

    public Function1(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Function1>();
    }

    [Function("Function1")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
    {
        // https://learn.microsoft.com/ja-jp/azure/app-service/reference-app-settings#scaling
        var instanceId = System.Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID");
        _logger.LogInformation("Run on {instanceId}.", instanceId);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString($"Welcome to Azure Functions on {instanceId}!");
        return response;
    }
}
