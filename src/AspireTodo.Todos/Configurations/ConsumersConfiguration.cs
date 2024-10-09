using MassTransit;
using AspireTodo.Todos.Features.TodoUsers.Consumers;

namespace AspireTodo.Todos.Configurations;

public static class ConsumersConfiguration
{
    public static void SetConsumers(this IBusRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<UserUpdatedConsumer>(ctx =>
        {
            ctx.UseConcurrentMessageLimit(200);
            ctx.UseConcurrencyLimit(200);
        });
    }
}