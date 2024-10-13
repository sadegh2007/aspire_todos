using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace AspireTodo.Core.MassTransit;

public abstract class SagaDbContext: DbContext
{
    protected SagaDbContext(DbContextOptions options): base(options)
    {
    }

    protected abstract IEnumerable<ISagaClassMap> Configurations { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var configuration in Configurations)
            configuration.Configure(modelBuilder);
    }
}