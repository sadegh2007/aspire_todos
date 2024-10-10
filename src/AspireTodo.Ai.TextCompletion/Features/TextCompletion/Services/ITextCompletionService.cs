using AspireTodo.Ai.TextCompletion.Shared;

namespace AspireTodo.Ai.TextCompletion.Features.TextCompletion.Services;

public interface ITextCompletionService
{
    public Task<TextCompletionGeneratedResponse> CompletionAsync(string input, CancellationToken cancellationToken = default);
}