using AspireTodo.Core.Data;
using AspireTodo.Todos.Data;
using Microsoft.EntityFrameworkCore;

namespace AspireTodo.Todos.Configurations;

public static class DatabaseConfiguration
{
    public static IHostApplicationBuilder AddAppDatabase(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<TodosDbContext>("TodosDb", configureDbContextOptions: options =>
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