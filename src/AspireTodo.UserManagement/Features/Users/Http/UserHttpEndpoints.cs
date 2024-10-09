using AspireTodo.Core.Shared;
using AspireTodo.UserManagement.Features.Users.Services;
using AspireTodo.UserManagement.Shared;

namespace AspireTodo.UserManagement.Features.Users.Http;

public static class UserHttpEndpoints
{
    public static IEndpointRouteBuilder MapUsers(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("users");

        api.MapGet("{id:int}", Get);

        return api;
    }

    private static async Task<UserDto> Get(UserId id, IUserService userService)
        => await userService.GetAsync(id);
}