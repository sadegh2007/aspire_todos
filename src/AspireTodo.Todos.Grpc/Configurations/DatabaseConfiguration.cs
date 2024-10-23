using AspireTodo.Core.Data;
using AspireTodo.Todos.Grpc.Data;
using Microsoft.EntityFrameworkCore;

namespace AspireTodo.Todos.Grpc.Configurations;

public static class DatabaseConfiguration
{
    public static WebApplicationBuilder AddAppDatabase(this WebApplicationBuilder builder)
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