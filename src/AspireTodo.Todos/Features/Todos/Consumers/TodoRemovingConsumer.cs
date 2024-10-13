using System.Text.Json;
using AspireTodo.Todos.Data;
using AspireTodo.Todos.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AspireTodo.Todos.Features.Todos.Consumers;

public partial class TodoRemovingConsumer(TodosDbContext appDbContext, ILogger<TodoRemovingConsumer> logger): IConsumer<TodoRemoving>
{
    private readonly ILogger<TodoRemovingConsumer> _logger = logger;

    [LoggerMessage(LogLevel.Information, Message = "TodoRemoving Received: {body}")]
    partial void LogBody(string body);

    [LoggerMessage(LogLevel.Information, Message = "TodoRemoving: todo with id of {id} removed.")]
    partial void LogCompleted(int id);

    public async Task Consume(ConsumeContext<TodoRemoving> context)
    {
        LogBody(JsonSerializer.Serialize(context.Message));

        var todo = await appDbContext.Todos
            .Where(x => x.Creator.UserId == context.Message.UserId)
            .FirstOrDefaultAsync(x => x.Id == context.Message.TodoId);

        if (todo == null)
        {
            await context.Publish<TodoNotFound>(new(context.Message.TodoId, context.Message.UserId));
            return;
        }

        appDbContext.Todos.Remove(todo);
        await appDbContext.SaveChangesAsync();

        LogCompleted(context.Message.TodoId.Value);

        await context.Publish<TodoRemoved>(new(context.Message.TodoId, context.Message.UserId));
    }
}