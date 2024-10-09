using AspireTodo.Core.Data.Abstractions;
using AspireTodo.Core.Shared;
using Microsoft.AspNetCore.Identity;

namespace AspireTodo.UserManagement.Features.Users.Domains;

public class Role: IdentityRole<int>, ICreatedAt, IUpdatedAt
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}