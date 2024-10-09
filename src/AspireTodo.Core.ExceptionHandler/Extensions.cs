using Microsoft.Extensions.DependencyInjection;

namespace AspireTodo.Core.ExceptionHandler;

public static class Extensions
{
    public static IServiceCollection AddTodoExceptionHandler(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<TodoExceptionHandler>();

        return services;
    }
}