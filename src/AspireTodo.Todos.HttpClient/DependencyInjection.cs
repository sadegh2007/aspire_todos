using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace AspireTodo.Todos.HttpClient;

public static class DependencyInjection
{
    private const string HttpClientName = "todos-http";

    public static IServiceCollection AddTodosHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient(HttpClientName, client => client.BaseAddress = new Uri("http://todos"));

        var settings = new RefitSettings();

        services.AddRefitClient<ITodosHttpApi>(_ => settings, HttpClientName);

        return services;
    }
}