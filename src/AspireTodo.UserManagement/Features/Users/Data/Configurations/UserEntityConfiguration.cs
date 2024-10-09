using AspireTodo.Core.Shared;
using AspireTodo.UserManagement.Features.Users.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspireTodo.UserManagement.Features.Users.Data.Configurations;

public class UserEntityConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        
        builder.Property(x => x.Name)
            .HasMaxLength(GlobalConstants.NameMaxLength);
        
        builder.Property(x => x.Family)
            .HasMaxLength(GlobalConstants.FamilyMaxLength);
        
        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(GlobalConstants.PhoneNumberMaxLength);

        builder.HasIndex(x => x.PhoneNumber).IsUnique();

        builder.HasIndex(x => x.DeletedAt);

        builder.HasQueryFilter(x => x.DeletedAt == null);
    }
}