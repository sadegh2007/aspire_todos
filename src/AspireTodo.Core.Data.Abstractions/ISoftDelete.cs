namespace AspireTodo.Core.Data.Abstractions;

public interface ISoftDelete
{
    public DateTimeOffset? DeletedAt { get; set; }
    public int? DeletedById { get; set; }
}