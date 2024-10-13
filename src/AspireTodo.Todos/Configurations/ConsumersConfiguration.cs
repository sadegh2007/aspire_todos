using AspireTodo.Todos.Data;
using AspireTodo.Todos.Features.Todos.Consumers;
using MassTransit;
using AspireTodo.Todos.Features.TodoUsers.Consumers;
using AspireTodo.Todos.StateMachines;
using AspireTodo.Todos.StateMachines.States;

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

        configurator.AddConsumer<TodoCreatingConsumer>(ctx =>
        {
            ctx.UseConcurrentMessageLimit(100);
            ctx.UseConcurrencyLimit(100);
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
        
        configurator.AddConsumer<TodoRemovingConsumer>(ctx =>
        {
            ctx.UseConcurrentMessageLimit(200);
            ctx.UseConcurrencyLimit(200);
        });
    }
}