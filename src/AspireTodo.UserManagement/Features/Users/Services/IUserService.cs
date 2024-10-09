using AspireTodo.Core.Shared;
using AspireTodo.UserManagement.Shared;

namespace AspireTodo.UserManagement.Features.Users.Services;

public interface IUserService
{
    public Task<UserDto> GetAsync(UserId userId, CancellationToken cancellationToken = default);
}