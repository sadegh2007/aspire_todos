using AspireTodo.UserManagement.Features.Auth.Services;
using AspireTodo.UserManagement.Features.Users.Services;
using Microsoft.AspNetCore.HttpLogging;

namespace AspireTodo.UserManagement.Configuration;

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

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}