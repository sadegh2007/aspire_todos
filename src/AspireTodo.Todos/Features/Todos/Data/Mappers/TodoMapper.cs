using AspireTodo.Todos.Features.Todos.Domains;
using AspireTodo.Todos.Shared;
using EntityFrameworkCore.Projectables;

namespace AspireTodo.Todos.Features.Todos.Data.Mappers;

public static class TodoMapper
{
    [Projectable(NullConditionalRewriteSupport = NullConditionalRewriteSupport.Rewrite)]
    public static TodoDto ToDto(this Todo todo) => new()
    {
        Id = todo.Id,
        Title = todo.Title,
        Summery = todo.Summery,
        IsCompleted = todo.IsCompleted,
        CompletedAt = todo.CompletedAt,
        CreatedAt = todo.CreatedAt
    };
}