using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.ChatCompletion;

namespace isolated_net9_semantic_kernel;

public class Function1
{
    private readonly ILogger<Function1> _logger;
    private readonly IChatCompletionService _chatService;

    public Function1(ILogger<Function1> logger, IChatCompletionService chatService)
    {
        _logger = logger;
        _chatService = chatService;
    }

    [Function("Function1")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
    {
        var prompt = "Microsoft Azureとはなんですか？";
        var completion = await _chatService.GetChatMessageContentAsync(prompt);
        return new OkObjectResult(completion);
    }
}
