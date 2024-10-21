using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace AspireTodo.UserManagement.HttpClient;

public static class DependencyInjection
{
    private const string HttpClientName = "users-http";

    public static IServiceCollection AddUsersHttpClients(this IServiceCollection services, string? url = null)
    {
        services.AddHttpClient(HttpClientName, client => client.BaseAddress = new Uri(url ?? "http://users"));

        var settings = new RefitSettings();

        services.AddRefitClient<IUsersHttpApi>(_ => settings, HttpClientName);
        services.AddRefitClient<IAccountsHttpApi>(_ => settings, HttpClientName);

        return services;
    }
}