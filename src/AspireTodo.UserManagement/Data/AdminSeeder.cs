using AspireTodo.UserManagement.Features.Users.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Remart.Core.Data;

namespace AspireTodo.UserManagement.Data;

public class AdminSeeder(UserManager<User> userManager): IDbSeeder<UsersDbContext>
{
    public async Task SeedAsync(UsersDbContext context)
    {
        var user = new User("09352760765")
        {
            Name = "Sadeq",
            Family = "Hajizadeh"
        };

        if (await context.Users.AnyAsync(x => x.PhoneNumber == user.PhoneNumber))
        {
            return;
        }

        await userManager.CreateAsync(user, "123456");
        await context.SaveChangesAsync();
    }
}