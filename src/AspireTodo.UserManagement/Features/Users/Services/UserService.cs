using AspireTodo.Core.Exceptions;
using AspireTodo.Core.Shared;
using AspireTodo.UserManagement.Data;
using AspireTodo.UserManagement.Features.Users.Data.Mappers;
using AspireTodo.UserManagement.Shared;
using Microsoft.EntityFrameworkCore;

namespace AspireTodo.UserManagement.Features.Users.Services;

public class UserService(UsersDbContext appDbContext): IUserService
{
    public async Task<UserDto> GetAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await appDbContext.Users.AsNoTracking()
            .Select(x => x.ToDto())
            .FirstOrDefaultAsync(x => x.Id == userId.Value, cancellationToken)
            ?? throw new NotFoundException("User not found.");
    }
}