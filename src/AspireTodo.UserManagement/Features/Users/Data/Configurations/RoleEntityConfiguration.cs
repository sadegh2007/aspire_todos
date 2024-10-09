using AspireTodo.Core.Shared;
using AspireTodo.UserManagement.Features.Users.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspireTodo.UserManagement.Features.Users.Data.Configurations;

public class RoleEntityConfiguration: IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasMaxLength(GlobalConstants.TitleMaxLength);
    }
}