using AspireTodo.Core.Data;
using AspireTodo.UserManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace AspireTodo.UserManagement.Configuration;

public static class DatabaseConfiguration
{
    public static IHostApplicationBuilder AddAppDatabase(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<UsersDbContext>("UsersDb", configureDbContextOptions: options =>
        {
            options.UseNpgsql();
            options.UseProjectables();
            options.AddInterceptors(new EntityHelperSaveChangeInterceptor());
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });

        return builder;
    }
}