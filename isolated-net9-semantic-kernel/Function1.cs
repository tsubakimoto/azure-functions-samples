using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace isolated_net9_semantic_kernel;

public class Function1
{
    private readonly ILogger<Function1> _logger;
    private readonly IChatCompletionService _chatService;
    private readonly Kernel _kernel;

    public Function1(ILogger<Function1> logger, IChatCompletionService chatService, Kernel kernel)
    {
        _logger = logger;
        _chatService = chatService;
        _kernel = kernel;
    }

    [Function("Function1")]
    public async Task<IActionResult> Run1([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
    {
        var prompt = "Microsoft Azureとはなんですか？";
        var completion = await _chatService.GetChatMessageContentAsync(prompt);
        return new OkObjectResult(completion);
    }

    [Function("Function2")]
    public async Task<IActionResult> Run2([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
    {
        PromptExecutionSettings settings = new AzureOpenAIPromptExecutionSettings()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };

        var prompt = "My name is Smith. How old am I?";
        var completion = await _chatService.GetChatMessageContentAsync(prompt, settings, _kernel);
        return new OkObjectResult(completion);
    }
}
