using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenAI;
using Microsoft.Azure.WebJobs.Extensions.OpenAI.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace inprocess_net6_openai;

public class WhoIs
{
    private readonly ILogger<WhoIs> _logger;

    public WhoIs(ILogger<WhoIs> log)
    {
        _logger = log;
    }

    [FunctionName(nameof(WhoIs))]
    [OpenApiOperation(operationId: nameof(WhoIs), tags: new[] { "name" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "name", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, Route = "whois/{name}")] HttpRequest req,
        [TextCompletion("Who is {name}?", Model = "gpt-35-turbo")] TextCompletionResponse response)
    {
        return new OkObjectResult(response.Content);
    }
}

