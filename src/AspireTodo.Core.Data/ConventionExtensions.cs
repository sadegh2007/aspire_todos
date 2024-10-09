using AspireTodo.Core.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AspireTodo.Core.Data;

public static class ConventionExtensions
{
    public static void AddStronglyTypedIdConventions(this ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<UserId>().HaveConversion<UserIdConvertor>();
        configurationBuilder.Properties<TodoId>().HaveConversion<TodoIdConvertor>();
        configurationBuilder.Properties<RoleId>().HaveConversion<RoleIdConvertor>();
    }

    private sealed class UserIdConvertor(): ValueConverter<UserId, int>(v => v.Value, v => UserId.FromInt32(v));
    private sealed class TodoIdConvertor(): ValueConverter<TodoId, int>(v => v.Value, v => TodoId.FromInt32(v));
    private sealed class RoleIdConvertor(): ValueConverter<RoleId, int>(v => v.Value, v => RoleId.FromInt32(v));
}