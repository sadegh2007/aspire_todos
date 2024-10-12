using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspireTodo.Todos.Features.Todos.Saga;

public class TodoStateMap: SagaClassMap<TodoState>
{
    protected override void Configure(EntityTypeBuilder<TodoState> entity, ModelBuilder model)
    {
        entity.HasKey(x => x.CorrelationId);

        base.Configure(entity, model);
    }
}