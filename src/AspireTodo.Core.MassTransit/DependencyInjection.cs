using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace AspireTodo.Core.MassTransit;

public static class DependencyInjection
{
    public static IServiceCollection AddTodoMassTransit(this IServiceCollection services, Action<IBusRegistrationConfigurator> configure)
    {
        var host = Environment.GetEnvironmentVariable("ConnectionStrings__EventBus");
        ArgumentNullException.ThrowIfNull(host);

        services.AddMassTransit(configurator =>
        {
            configurator.SetKebabCaseEndpointNameFormatter();

            configure.Invoke(configurator);

            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.AutoStart = true;

                cfg.UseInstrumentation();

                cfg.Host(new Uri(host), "/");
                cfg.SetQuorumQueue(3);
                cfg.UseInMemoryOutbox(context);
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}