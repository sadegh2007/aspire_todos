using AspireTodo.Notifications.Consumers;
using MassTransit;

namespace AspireTodo.Notifications.Configurations;

public static class ConsumersConfiguration
{
    public static void SetConsumers(this IBusRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<TodoCreatedConsumer>(ctx =>
        {
            ctx.UseConcurrentMessageLimit(100);
            ctx.UseConcurrencyLimit(100);
        })
        .Endpoint(e =>
        {
            e.InstanceId = Guid.NewGuid().ToString();
            e.Temporary = true;
        });

        configurator.AddConsumer<FailedUserUpdateTodosCountConsumer>(ctx =>
        {
            ctx.UseConcurrentMessageLimit(100);
            ctx.UseConcurrencyLimit(100);
        })
        .Endpoint(e =>
        {
            e.InstanceId = Guid.NewGuid().ToString();
            e.Temporary = true;
        });

        configurator.AddConsumer<TodoNotFoundConsumer>(ctx =>
            {
                ctx.UseConcurrentMessageLimit(100);
                ctx.UseConcurrencyLimit(100);
            })
            .Endpoint(e =>
            {
                e.InstanceId = Guid.NewGuid().ToString();
                e.Temporary = true;
            });

        configurator.AddConsumer<TodoRemovedConsumer>(ctx =>
            {
                ctx.UseConcurrentMessageLimit(100);
                ctx.UseConcurrencyLimit(100);
            })
            .Endpoint(e =>
            {
                e.InstanceId = Guid.NewGuid().ToString();
                e.Temporary = true;
            });
    }
}