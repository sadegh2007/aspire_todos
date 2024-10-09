using MassTransit;
using System.Text.Json;
using AspireTodo.Core.Shared;
using AspireTodo.Todos.Data;
using Microsoft.EntityFrameworkCore;
using AspireTodo.UserManagement.Events;

namespace AspireTodo.Todos.Features.TodoUsers.Consumers;

public partial class UserUpdatedConsumer(
    TodosDbContext appDbContext,
    ILogger<UserUpdatedConsumer> logger): IConsumer<UserUpdated>
{
    private readonly ILogger<UserUpdatedConsumer> _logger = logger;

    [LoggerMessage(LogLevel.Information, "User Updated Received: {message}")]
    partial void LogUserUpdatedReceived(string message);

    [LoggerMessage(LogLevel.Information, "User with id {userId} not exists in this database")]
    partial void LogUserNotExists(UserId userId);

    public async Task Consume(ConsumeContext<UserUpdated> context)
    {
        LogUserUpdatedReceived(JsonSerializer.Serialize(context.Message));

        var user = await appDbContext.TodoUsers.FirstOrDefaultAsync(x => x.UserId == context.Message.UserId);

        if (user != null)
        {
            user.Name = context.Message.Name;
            user.Family = context.Message.Family;

            appDbContext.TodoUsers.Update(user);
            await appDbContext.SaveChangesAsync();

            return;
        }

        LogUserNotExists(context.Message.UserId);
    }
}