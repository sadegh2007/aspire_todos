using System.Text.Json;
using AspireTodo.UserManagement.Data;
using AspireTodo.UserManagement.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AspireTodo.UserManagement.Features.Users.Consumers;

public partial class TodoCreatedConsumer(UsersDbContext appDbContext, ILogger<TodoCreatedConsumer> logger): IConsumer<Todos.Events.TodoCreated>
{
    [LoggerMessage(LogLevel.Information, "TodoCreated Received: {body}")]
    partial void LogReceivedMessage(string body);
    
    [LoggerMessage(LogLevel.Information, "TodoCreated User not found with id {userId}")]
    partial void LogUserNotFound(int userId);
    
    [LoggerMessage(LogLevel.Information, "TodoCreated User {userId} updated with {todosCount} todos count")]
    partial void LogUserUpdated(int userId, int todosCount);
    
    public async Task Consume(ConsumeContext<Todos.Events.TodoCreated> context)
    {
        LogReceivedMessage(JsonSerializer.Serialize(context.Message));

        var user = await appDbContext.Users.FirstOrDefaultAsync(x => x.Id == context.Message.UserId.Value);

        if (user == null)
        {
            LogUserNotFound(context.Message.UserId.Value);
            await context.Publish<FailedUserUpdateTodosCount>(new(context.Message.UserId, context.Message.TodoId));
            return;
        }

        user.TodosCount = context.Message.UserTodosCount;

        await appDbContext.SaveChangesAsync();

        LogUserUpdated(context.Message.UserId.Value, context.Message.UserTodosCount);

        await context.Publish<UserUpdatedTodosCount>(new(context.Message.UserId, context.Message.TodoId, user.TodosCount));
    }
}