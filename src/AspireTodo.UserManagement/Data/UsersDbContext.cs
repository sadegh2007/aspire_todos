using AspireTodo.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using AspireTodo.UserManagement.Features.Users.Domains;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AspireTodo.UserManagement.Data;

public class UsersDbContext(DbContextOptions<UsersDbContext> options): DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
        base.OnModelCreating(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.AddStronglyTypedIdConventions();
    }
}

public class UsersDbContextFactory : IDesignTimeDbContextFactory<UsersDbContext>
{
    private const string Cs = "Server=127.0.0.1;Port=5432;Database=remart.users;User Id=postgres;Password=123456;Include Error Detail=True";

    public UsersDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<UsersDbContext>();
        builder.UseNpgsql(Cs);

        return new UsersDbContext(builder.Options);
    }
}