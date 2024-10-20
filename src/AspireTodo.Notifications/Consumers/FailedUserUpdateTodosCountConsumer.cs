using System.Text.Json;
using AspireTodo.Notifications.Hubs;
using AspireTodo.Notifications.Shared;
using AspireTodo.UserManagement.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace AspireTodo.Notifications.Consumers;

public partial class FailedUserUpdateTodosCountConsumer(
    IHubContext<NotificationsHub> hubContext,
    ILogger<FailedUserUpdateTodosCountConsumer> logger
): IConsumer<FailedUserUpdateTodosCount>
{
    [LoggerMessage(LogLevel.Information, Message = "Notification: FailedUserUpdateTodosCountConsumer Received {body} send to {groupName}")]
    partial void LoggerReceived(string body, string groupName);

    public async Task Consume(ConsumeContext<FailedUserUpdateTodosCount> context)
    {
        var groupName = NotificationsHub.GroupName(context.Message.UserId);

        LoggerReceived(JsonSerializer.Serialize(context.Message), groupName);

        await hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", new NotificationBaseData<FailedUserUpdateTodosCount>
        {
            Data = context.Message,
        });
    }
}