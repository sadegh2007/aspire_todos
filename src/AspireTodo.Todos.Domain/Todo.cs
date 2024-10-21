using AspireTodo.Core.Data.Abstractions;
using AspireTodo.Core.Shared;

namespace AspireTodo.Todos.Domain;

public class Todo: IModel<TodoId>, ICreatedAt, IUpdatedAt, ISoftDelete
{
    private Todo() { }

    public static Todo Create(string title, string? summery = null)
    {
        return new Todo
        {
            Title = title,
            Summery = summery
        };
    }

    public TodoId Id { get; set; }
    public string Title { get; set; }
    public string? Summery { get; set; }

    public bool IsCompleted { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }

    public int CreatorId { get; set; }
    public TodoUser Creator { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public int? DeletedById { get; set; }
}