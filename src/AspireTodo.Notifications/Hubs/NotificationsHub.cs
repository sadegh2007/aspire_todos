using AspireTodo.Core.Identity;
using AspireTodo.Core.Shared;
using Microsoft.AspNetCore.SignalR;

namespace AspireTodo.Notifications.Hubs;

public class NotificationsHub(IHttpContextAccessor httpContextAccessor): Hub<INotificationsHub>
{
    public static string GroupName(UserId userId) => $"Todo-{userId.Value}";

    public override async Task OnConnectedAsync()
    {
        var userId = httpContextAccessor.HttpContext?.User.GetTypedUserId();

        if (userId != null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GroupName(userId.Value));
        }
    }
}

public interface INotificationsHub
{
    public Task ReceiveMessage(string message);
}