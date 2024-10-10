using AspireTodo.Ai.TextCompletion.Features.TextCompletion.Services;
using AspireTodo.Todos.HttpClient;

namespace AspireTodo.Ai.TextCompletion.Configurations;

public static class ServicesConfiguration
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ITextCompletionService, TextCompletionService>();

        services.AddTodosHttpClients();

        return services;
    }
}