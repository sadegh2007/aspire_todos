using System.Security.Claims;
using AspireTodo.Core.Shared;

namespace AspireTodo.Core.Identity;

public static class AuthExtensions
{
    public static int? GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirst("sub") ?? claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
        return userId is not null ? int.Parse(userId.Value) : null;
    }
}