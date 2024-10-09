using AspireTodo.UserManagement.Features.Users.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Remart.Core.Data;

namespace AspireTodo.UserManagement.Data;

public class AdminSeeder(UserManager<User> userManager): IDbSeeder<UsersDbContext>
{
    public async Task SeedAsync(UsersDbContext context)
    {
        var user = new User("09333333333")
        {
            Name = "Sadeq",
            Family = "Hajizadeh"
        };

        if (await context.Users.AnyAsync(x => x.PhoneNumber == user.PhoneNumber))
        {
            return;
        }

        await userManager.CreateAsync(user, "a2345g");
        await context.SaveChangesAsync();
    }
}