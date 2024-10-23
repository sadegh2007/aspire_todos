using AspireTodo.Core.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AspireTodo.Core.Data;

public class EntityHelperSaveChangeInterceptor: SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        foreach (var entityEntry in eventData.Context?.ChangeTracker.Entries()!)
        {
            switch (entityEntry.State)
            {
                case EntityState.Deleted:
                    if (entityEntry.Entity is ISoftDelete softDelete)
                    {
                        entityEntry.State = EntityState.Modified;
                        softDelete.DeletedAt = DateTimeOffset.UtcNow;
                    }

                    break;
                case EntityState.Modified:
                    if (entityEntry.Entity is IUpdatedAt updatedAt)
                    {
                        updatedAt.UpdatedAt = DateTimeOffset.UtcNow;
                    }

                    break;
                case EntityState.Added:

                    if (entityEntry.Entity is ICreatedAt createdAt)
                    {
                        createdAt.CreatedAt = DateTimeOffset.UtcNow;
                    }

                    break;
            }
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}