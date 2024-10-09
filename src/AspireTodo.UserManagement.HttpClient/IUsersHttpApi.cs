using AspireTodo.UserManagement.Shared;
using Refit;

namespace AspireTodo.UserManagement.HttpClient;

public interface IUsersHttpApi
{
    [Get("/api/users/{id}")]
    public Task<UserDto> GetAsync(int id, [Authorize] string token, CancellationToken cancellationToken = default);
}