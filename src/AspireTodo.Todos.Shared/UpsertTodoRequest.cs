using AspireTodo.Core.Shared;
using FluentValidation;

namespace AspireTodo.Todos.Shared;

public class UpsertTodoRequest
{
    public string Title { get; set; }
    public string? Summery { get; set; }
}

public class UpsertTodoRequestValidator : AbstractValidator<UpsertTodoRequest>
{
    public UpsertTodoRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().NotNull().MaximumLength(GlobalConstants.TitleMaxLength);
        RuleFor(x => x.Summery).NotEmpty().NotNull().MaximumLength(GlobalConstants.SummeryMaxLength);
    }
}