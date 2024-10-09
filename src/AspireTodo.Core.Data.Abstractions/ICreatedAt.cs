namespace AspireTodo.Core.Data.Abstractions;

public interface ICreatedAt
{
    public DateTimeOffset CreatedAt { get; set; }
}