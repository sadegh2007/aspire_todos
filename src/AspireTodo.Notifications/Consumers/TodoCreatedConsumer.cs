using System.Text.Json;
using AspireTodo.Notifications.Hubs;
using AspireTodo.Notifications.Models;
using AspireTodo.Todos.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace AspireTodo.Notifications.Consumers;

public partial class TodoCreatedConsumer(
    IHubContext<NotificationsHub> hubContext,
    ILogger<TodoCreatedConsumer> logger): IConsumer<TodoCreated>
{
    [LoggerMessage(LogLevel.Information, Message = "Notification: TodoCreated Received: {body}")]
    partial void LogReceivedBody(string body);

    public async Task Consume(ConsumeContext<TodoCreated> context)
    {
        LogReceivedBody(JsonSerializer.Serialize(context.Message));

        var groupName = NotificationsHub.GroupName(context.Message.UserId);
        await hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", new NotificationBaseData<TodoCreated>
        {
            Data = context.Message,
        });
    }
}