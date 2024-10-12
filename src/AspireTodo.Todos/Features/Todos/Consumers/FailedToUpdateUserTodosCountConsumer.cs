using AspireTodo.Todos.Data;
using AspireTodo.UserManagement.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AspireTodo.Todos.Features.Todos.Consumers;

public partial class FailedToUpdateUserTodosCountConsumer(
    TodosDbContext appDbContext,
    ILogger<FailedToUpdateUserTodosCountConsumer> logger
): IConsumer<FailedUserUpdateTodosCount>
{
    [LoggerMessage(LogLevel.Information, "FailedUserUpdateTodosCount Received")]
    partial void LogEventReceived();
    
    [LoggerMessage(LogLevel.Information, "FailedUserUpdateTodosCount Todo {id} has been removed")]
    partial void LogTodoRemoved(int id);

    public async Task Consume(ConsumeContext<FailedUserUpdateTodosCount> context)
    {
        LogEventReceived();

        var todo = await appDbContext.Todos.FirstOrDefaultAsync(x => x.Id == context.Message.TodoId);

        if (todo == null)
        {
            return;
        }

        appDbContext.Todos.Remove(todo);
        await appDbContext.SaveChangesAsync();

        LogTodoRemoved(todo.Id.Value);
    }
}