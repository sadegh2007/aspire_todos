using AspireTodo.Ai.TextCompletion.Features.TextCompletion.Http;

namespace AspireTodo.Ai.TextCompletion;

public static class Router
{
    public static void MapRouter(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("api")
            .WithGroupName("v1")
            .RequireAuthorization();

        api.MapTextCompletion();
    }
}