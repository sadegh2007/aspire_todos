using AspireTodo.Core.Shared;

namespace AspireTodo.Todos.Shared;

public class TodoDto
{
    public TodoId Id { get; set; }
    public string Title { get; set; }
    public string? Summery { get; set; }
    public bool IsCompleted { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}