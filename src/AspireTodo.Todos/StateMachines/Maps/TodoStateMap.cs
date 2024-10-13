using AspireTodo.Todos.StateMachines.States;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspireTodo.Todos.StateMachines.Maps;

public class TodoStateMap: SagaClassMap<TodoState>
{
    protected override void Configure(EntityTypeBuilder<TodoState> entity, ModelBuilder model)
    {
        entity.HasKey(x => x.CorrelationId);
        entity.OwnsOne(x => x.Todo);

        base.Configure(entity, model);
    }
}