using FluentValidation;

namespace AspireTodo.Todos.Shared;

public class MarkAsCompletedRequest
{
    public bool IsCompleted { get; set; }
}

public class MarkAsCompletedRequestValidator : AbstractValidator<MarkAsCompletedRequest>
{
    public MarkAsCompletedRequestValidator()
    {
        RuleFor(x => x.IsCompleted).NotNull();
    }
}