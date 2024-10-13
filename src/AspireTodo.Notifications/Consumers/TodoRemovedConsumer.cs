using AspireTodo.Notifications.Hubs;
using AspireTodo.Notifications.Models;
using MassTransit;
using AspireTodo.Todos.Events;
using Microsoft.AspNetCore.SignalR;

namespace AspireTodo.Notifications.Consumers;

public class TodoRemovedConsumer(IHubContext<NotificationsHub> hubContext): IConsumer<TodoRemoved>
{
    public async Task Consume(ConsumeContext<TodoRemoved> context)
    {
        var groupName = NotificationsHub.GroupName(context.Message.UserId);
        await hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", new NotificationBaseData<TodoRemoved>
        {
            Data = context.Message,
        });
    }
}