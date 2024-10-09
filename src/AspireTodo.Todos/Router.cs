using AspireTodo.Todos.Features.Todos.Http;

namespace AspireTodo.Todos;

public static class Router
{
    public static void MapRoutes(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("api")
            .WithGroupName("v1")
            .RequireAuthorization();

        api.MapTodos();
    }
}