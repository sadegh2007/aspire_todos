using AspireTodo.Core.Data.Abstractions;
using AspireTodo.Core.Shared;
using Microsoft.AspNetCore.Identity;

namespace AspireTodo.UserManagement.Features.Users.Domains;

public sealed class User: IdentityUser<int>, ICreatedAt, IUpdatedAt, ISoftDelete
{
    public User(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
        UserName = phoneNumber;
    }

    public string? Name { get; set; }
    public string? Family { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public int? DeletedById { get; set; }
}