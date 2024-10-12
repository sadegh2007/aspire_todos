using AspireTodo.Core.Data;
using AspireTodo.Todos.Features.Todos.Domains;
using AspireTodo.Todos.Features.Todos.Saga;
using AspireTodo.Todos.Features.TodoUsers.Domains;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SagaDbContext = AspireTodo.Core.MassTransit.SagaDbContext;

namespace AspireTodo.Todos.Data;

public class TodosDbContext(DbContextOptions<SagaDbContext> options) : SagaDbContext(options)
{
    public DbSet<Todo> Todos => Set<Todo>();
    public DbSet<TodoUser> TodoUsers => Set<TodoUser>();
    public DbSet<TodoState> TodoStates => Set<TodoState>();

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new TodoStateMap(); }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodosDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.AddStronglyTypedIdConventions();
    }
}

public class TodosDbContextFactory : IDesignTimeDbContextFactory<TodosDbContext>
{
    private const string Cs = "Server=127.0.0.1;Port=5432;Database=remart.users;User Id=postgres;Password=123456;Include Error Detail=True";

    public TodosDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<SagaDbContext>();
        builder.UseNpgsql(Cs);

        return new TodosDbContext(builder.Options);
    }
}