using AspireTodo.Todos.Events;
using AspireTodo.UserManagement.Data;
using AspireTodo.UserManagement.Features.Users.Consumers;
using MassTransit;

namespace AspireTodo.UserManagement.Configuration;

public static class ConsumersConfiguration
{
    public static void SetConsumers(this IBusRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<TodoCreatedConsumer>(ctx =>
        {
            ctx.UseConcurrentMessageLimit(200);
            ctx.UseConcurrencyLimit(200);
        });
    }
}