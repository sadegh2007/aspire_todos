using AspireTodo.Ai.TextCompletion.Features.TextCompletion.Services;
using AspireTodo.Ai.TextCompletion.Shared;
using Microsoft.AspNetCore.Mvc;

namespace AspireTodo.Ai.TextCompletion.Features.TextCompletion.Http;

public static class TextCompletionEndpoint
{
    public static IEndpointRouteBuilder MapTextCompletion(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("textCompletion");

        api.MapPost("", GetTextCompletion);

        return api;
    }

    private static async Task<TextCompletionGeneratedResponse> GetTextCompletion([FromBody] TextCompletionRequest request, ITextCompletionService textCompletionService)
        => await textCompletionService.CompletionAsync(request.Input);
}