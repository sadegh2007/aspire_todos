using AspireTodo.Core.Shared;
using AspireTodo.Todos.Features.Todos.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspireTodo.Todos.Features.Todos.Data.Configuration;

public class TodoEntityConfiguration: IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Title).HasMaxLength(GlobalConstants.TitleMaxLength);
        builder.Property(x => x.Summery).HasMaxLength(GlobalConstants.SummeryMaxLength);

        builder.HasIndex(x => x.IsCompleted);

        builder.HasIndex(x => x.DeletedAt);
        builder.HasQueryFilter(x => x.DeletedAt == null);
    }
}