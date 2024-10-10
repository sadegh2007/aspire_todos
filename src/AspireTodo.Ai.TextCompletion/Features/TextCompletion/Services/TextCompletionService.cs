using AspireTodo.Ai.TextCompletion.Shared;
using AspireTodo.Todos.HttpClient;
using Gridify;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AspireTodo.Ai.TextCompletion.Features.TextCompletion.Services;

public class TextCompletionService: ITextCompletionService
{
    private readonly ITodosHttpApi _todosHttpApi;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string Endpoint = "http://localhost:11434";

    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletionService;

    public TextCompletionService(ITodosHttpApi todosHttpApi, IHttpContextAccessor httpContextAccessor)
    {
        _todosHttpApi = todosHttpApi;
        _httpContextAccessor = httpContextAccessor;

        #pragma warning disable SKEXP0010
        var kernelBuilder = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion("qwen2.5:0.5b", apiKey: null, endpoint: new Uri(Endpoint));
        #pragma warning restore SKEXP0010

        _kernel = kernelBuilder.Build();
        _chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
    }

    public async Task<TextCompletionGeneratedResponse> CompletionAsync(string input, CancellationToken cancellationToken = default)
    {
        var chatHistory = new ChatHistory();

        var token = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var latestTodos = await _todosHttpApi.ListAsync(new GridifyQuery()
        {
            Page = 1,
            PageSize = 100,
        }, token);

        chatHistory.AddSystemMessage(
            """
            we are provider todos app service
            your assistant to help user to complete the inputted sentence with few words.
            please suggest the next phrase base on user input. with maximum 8 words in one line.
            do not ask any questions about the text user provided.
            """);

        chatHistory.AddSystemMessage("this is the previous todos of user");

        foreach (var todo in latestTodos.Data)
        {
            chatHistory.AddSystemMessage(todo.Title);
        }

        chatHistory.AddMessage(AuthorRole.User , input);

        var response = await _chatCompletionService.GetChatMessageContentAsync(
            chatHistory,
            kernel: _kernel,
            cancellationToken: cancellationToken);

        return new()
        {
            Text = response.Content ?? string.Empty
        };
    }
}