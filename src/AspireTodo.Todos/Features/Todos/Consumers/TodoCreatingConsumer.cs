using System.Text.Json;
using AspireTodo.Todos.Data;
using AspireTodo.Todos.Domain;
using AspireTodo.Todos.Events;
using AspireTodo.Todos.Features.Todos.Data.Mappers;
using AspireTodo.UserManagement.HttpClient;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AspireTodo.Todos.Features.Todos.Consumers;

public partial class TodoCreatingConsumer(
    TodosDbContext appDbContext,
    IUsersHttpApi usersHttpApi,
    ILogger<TodoCreatingConsumer> logger
): IConsumer<TodoCreating>
{
    [LoggerMessage(LogLevel.Information, Message = "TodoCreating Received: {body}")]
    partial void LogReceivedBody(string body);

    public async Task Consume(ConsumeContext<TodoCreating> context)
    {
        LogReceivedBody(JsonSerializer.Serialize(new
        {
            context.Message.Title,
            context.Message.Summery,
            context.Message.UserId,
        }));

        var todo = Todo.Create(context.Message.Title, context.Message.Summery);

        var user = await appDbContext.TodoUsers.FirstOrDefaultAsync(x => x.UserId == context.Message.UserId);
        if (user is null)
        {
            var appUser = await usersHttpApi.GetAsync(context.Message.UserId.Value, context.Message.AuthToken);
            user = TodoUser.Create(appUser);

            await appDbContext.TodoUsers.AddAsync(user);
        }

        todo.Creator = user;

        await appDbContext.Todos.AddAsync(todo);
        await appDbContext.SaveChangesAsync();

        var todoDto = await appDbContext.Todos.Select(x => x.ToDto())
            .FirstOrDefaultAsync(x => x.Id == todo.Id);

        if (todoDto == null)
        {
            // TODO: 
            return;
        }

        var todosCount = await appDbContext.Todos.Where(x => x.Creator.UserId == user.UserId).CountAsync();

        await context.Publish<TodoCreated>(new(todoDto, user.UserId, todosCount));
    }
}