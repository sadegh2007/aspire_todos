using AspireTodo.UserManagement.Shared;
using Refit;

namespace AspireTodo.UserManagement.HttpClient;

public interface IUserHttpApi
{
    [Get("/api/users")]
    public Task<UserDto> Get(int userId, CancellationToken token = default);
}