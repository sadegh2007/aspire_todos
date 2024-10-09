using AspireTodo.Core.Data;
using AspireTodo.Todos.Features.Todos.Domains;
using AspireTodo.Todos.Features.TodoUsers.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AspireTodo.Todos.Data;

public class TodosDbContext(DbContextOptions<TodosDbContext> options): DbContext(options)
{
    public DbSet<Todo> Todos => Set<Todo>();
    public DbSet<TodoUser> TodoUsers => Set<TodoUser>();
    
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
        var builder = new DbContextOptionsBuilder<TodosDbContext>();
        builder.UseNpgsql(Cs);

        return new TodosDbContext(builder.Options);
    }
}