namespace AspireTodo.Ai.TextCompletion.Shared;
using FluentValidation;

public class TextCompletionRequest
{
    public string Input { get; set; }
}

public class TextCompletionRequestValidator : AbstractValidator<TextCompletionRequest>
{
    public TextCompletionRequestValidator()
    {
        RuleFor(r => r.Input).NotNull();
    }
}