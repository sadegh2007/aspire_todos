using AspireTodo.UserManagement.Features.Auth.Http;
using AspireTodo.UserManagement.Features.Users.Http;

namespace AspireTodo.UserManagement;

public static class Router
{
    public static void MapAppRouter(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api")
            .WithGroupName("v1")
            .RequireAuthorization();

        api.MapAccounts();
        api.MapUsers();
    }
}