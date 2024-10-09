using AspireTodo.Todos.Features.Todos.Services;
using AspireTodo.UserManagement.HttpClient;
using Microsoft.AspNetCore.HttpLogging;

namespace AspireTodo.Todos.Configurations;

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

        services.AddUsersHttpClients();
        
        services.AddScoped<ITodoService, TodoService>();

        return services;
    }
}