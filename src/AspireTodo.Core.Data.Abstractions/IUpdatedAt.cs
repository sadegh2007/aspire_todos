namespace AspireTodo.Core.Data.Abstractions;

public interface IUpdatedAt
{
    public DateTimeOffset? UpdatedAt { get; set; }
}