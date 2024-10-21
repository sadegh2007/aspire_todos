using AspireTodo.Core.Shared;
using AspireTodo.Todos.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspireTodo.Todos.Features.TodoUsers.Data.Configuration;

public class TodoUserEntityConfiguration: IEntityTypeConfiguration<TodoUser>
{
    public void Configure(EntityTypeBuilder<TodoUser> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(GlobalConstants.NameMaxLength);
        builder.Property(x => x.Family).HasMaxLength(GlobalConstants.FamilyMaxLength);

        builder.HasIndex(x => x.UserId)
            .IsUnique();
    }
}