using AspireTodo.Todos.Data;
using AspireTodo.Todos.Features.Todos.Consumers;
using AspireTodo.Todos.Features.Todos.Saga;
using MassTransit;
using AspireTodo.Todos.Features.TodoUsers.Consumers;

namespace AspireTodo.Todos.Configurations;

public static class ConsumersConfiguration
{
    public static void SetConsumers(this IBusRegistrationConfigurator configurator)
    {
        configurator.AddSagaStateMachine<TodoStateMachine, TodoState>()
            .EntityFrameworkRepository(efc =>
            {
                efc.ExistingDbContext<TodosDbContext>();
                efc.UsePostgres();
            });

        configurator.AddConsumer<UserUpdatedConsumer>(ctx =>
        {
            ctx.UseConcurrentMessageLimit(200);
            ctx.UseConcurrencyLimit(200);
        });

        configurator.AddConsumer<FailedToUpdateUserTodosCountConsumer>(ctx =>
        {
            ctx.UseConcurrentMessageLimit(200);
            ctx.UseConcurrencyLimit(200);
        });
    }
}