using AspireTodo.Notifications.Hubs;
using AspireTodo.Notifications.Models;
using AspireTodo.Todos.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace AspireTodo.Notifications.Consumers;

public class TodoNotFoundConsumer(IHubContext<NotificationsHub> hubContext): IConsumer<TodoNotFound>
{
    public async Task Consume(ConsumeContext<TodoNotFound> context)
    {
        var groupName = NotificationsHub.GroupName(context.Message.UserId);
        await hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", new NotificationBaseData<TodoNotFound>
        {
            Data = context.Message,
        });
    }
}