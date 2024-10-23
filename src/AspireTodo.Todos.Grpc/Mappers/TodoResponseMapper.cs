using AspireTodo.Todos.Domain;
using EntityFrameworkCore.Projectables;

namespace AspireTodo.Todos.Grpc.Mappers;

public static class TodoResponseMapper
{
    [Projectable(NullConditionalRewriteSupport = NullConditionalRewriteSupport.Rewrite)]
    public static TodoGetResponse ToDto(this Todo todo) => new()
    {
        Title = todo.Title,
        Summery = todo.Summery ?? "",
        CreatedAt = todo.CreatedAt.ToString("yyyy-M-d HH:mm:ss"),
        IsCompleted = todo.IsCompleted,
        Id = todo.Id.Value
    };
}