using AspireTodo.Core.Shared;

namespace AspireTodo.Core.Data.Abstractions;

public interface ICreator
{
    public UserId CreatorId { get; set; }
}