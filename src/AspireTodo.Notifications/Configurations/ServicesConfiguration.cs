using AspireTodo.Todos.HttpClient;
using Microsoft.AspNetCore.HttpLogging;

namespace AspireTodo.Notifications.Configurations;

public static class ServicesConfiguration
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.All;
            options.CombineLogs = true;
        });

        services.AddTodosHttpClients();

        return services;
    }
}