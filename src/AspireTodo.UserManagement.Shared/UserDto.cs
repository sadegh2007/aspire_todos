using AspireTodo.Core.Shared;

namespace AspireTodo.UserManagement.Shared;

public class UserDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Family { get; set; }
    public string PhoneNumber { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}