using AspireTodo.UserManagement.Features.Users.Domains;
using AspireTodo.UserManagement.Shared;
using EntityFrameworkCore.Projectables;

namespace AspireTodo.UserManagement.Features.Users.Data.Mappers;

public static class UserMapper
{
    [Projectable(NullConditionalRewriteSupport = NullConditionalRewriteSupport.Rewrite)]
    public static UserDto ToDto(this User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Family = user.Family,
        PhoneNumber = user.PhoneNumber!,
        CreatedAt = user.CreatedAt
    };
}