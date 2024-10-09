using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace AspireTodo.UserManagement.HttpClient;

public static class DependencyInjection
{
    private const string HttpClientName = "users-http";

    public static IServiceCollection AddUsersHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient(HttpClientName, client => client.BaseAddress = new Uri("http://users"));

        var settings = new RefitSettings();

        services.AddRefitClient<IUsersHttpApi>(_ => settings, HttpClientName);

        return services;
    }
}