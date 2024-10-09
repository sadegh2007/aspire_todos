using AspireTodo.Core.Shared;
using AspireTodo.UserManagement.Shared;
using AspireTodo.Core.Data.Abstractions;

namespace AspireTodo.Todos.Features.TodoUsers.Domains;

public class TodoUser: IModel, ICreatedAt, IUpdatedAt
{
    private TodoUser() { }

    public static TodoUser Create(UserDto appUser)
    {
        return new()
        {
            UserId = UserId.FromInt32(appUser.Id),
            Name = appUser.Name,
            Family = appUser.Family,
        };
    }

    public int Id { get; set; }
    public UserId UserId { get; set; }
    public string? Name { get; set; }
    public string? Family { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}