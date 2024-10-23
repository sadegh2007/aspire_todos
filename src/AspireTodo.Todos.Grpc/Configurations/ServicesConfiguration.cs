using Microsoft.AspNetCore.HttpLogging;

namespace AspireTodo.Todos.Grpc.Configurations;

public static class ServicesConfiguration
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddHttpLogging(options =>
        {
            options.CombineLogs = true;
            options.LoggingFields = HttpLoggingFields.All;
        });

        return services;
    }
}