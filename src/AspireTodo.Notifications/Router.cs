using AspireTodo.Core.Identity;
using AspireTodo.Notifications.Hubs;
using AspireTodo.Notifications.Shared;
using Microsoft.AspNetCore.SignalR;

namespace AspireTodo.Notifications;

public static class Router
{
    public static void MapRoutes(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("api")
            .WithGroupName("v1")
            .RequireAuthorization();

        api.MapGet("notifications/test", Test);
    }

    private static async Task Test(IHubContext<NotificationsHub> hubContext, IHttpContextAccessor httpContextAccessor)
    {
        var userId = httpContextAccessor.HttpContext!.User.GetTypedUserId()!.Value;

        var groupName = NotificationsHub.GroupName(userId);
        await hubContext.Clients.Group(groupName)
            .SendAsync("ReceiveMessage", new NotificationBaseData<object>()
            {
                Data = new { UserId = userId, Group = groupName }
            });
    }
}