using Microsoft.EntityFrameworkCore;

namespace Remart.Core.Data;

public interface IDbSeeder<in TContext> where TContext : DbContext
{
	Task SeedAsync(TContext context);
}
