using AspireTodo.Core.Shared;
using Microsoft.AspNetCore.Identity;
using AspireTodo.UserManagement.Data;
using AspireTodo.UserManagement.Features.Users.Domains;

namespace AspireTodo.UserManagement.Configuration;

public static class IdentityConfiguration
{
    public static IServiceCollection AddAppIdentity(this IServiceCollection services) {
        services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = GlobalConstants.MinPasswordLength;
            })
            .AddEntityFrameworkStores<UsersDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}