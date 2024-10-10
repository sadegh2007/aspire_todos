using AspireTodo.Core.Exceptions;
using AspireTodo.Core.Identity;
using AspireTodo.Core.Shared;
using AspireTodo.Todos.Data;
using AspireTodo.Todos.Exceptions;
using AspireTodo.Todos.Features.Todos.Data.Mappers;
using AspireTodo.Todos.Features.Todos.Domains;
using AspireTodo.Todos.Features.TodoUsers.Domains;
using AspireTodo.Todos.Shared;
using AspireTodo.UserManagement.HttpClient;
using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace AspireTodo.Todos.Features.Todos.Services;

public class TodoService(
    TodosDbContext dbContext,
    IHttpContextAccessor httpContextAccessor,
    IUsersHttpApi usersHttpApi
): ITodoService
{
    public async Task<Paging<TodoDto>> ListAsync(GridifyQuery query, CancellationToken cancellationToken = default)
    {
        var userId = httpContextAccessor.HttpContext!.User.GetTypedUserId()!.Value;

        return await dbContext.Todos.AsNoTracking()
            .Where(x => x.Creator.UserId == userId)
            .Select(x => x.ToDto())
            .GridifyAsync(query, cancellationToken);
    }

    private async Task<Todo> GetTodoAsync(TodoId id, CancellationToken cancellationToken = default)
    {
        var userId = httpContextAccessor.HttpContext!.User.GetTypedUserId();
        return await dbContext.Todos.AsNoTracking()
                   .Where(x => x.Creator.UserId == userId)
                   .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
               ?? throw new TodosNotFound();
    }

    public async Task<TodoDto> GetAsync(TodoId id, CancellationToken cancellationToken = default)
    {
        var userId = httpContextAccessor.HttpContext!.User.GetTypedUserId();

        return await dbContext.Todos.AsNoTracking()
                   .Where(x => x.Creator.UserId == userId)
                   .Select(x => x.ToDto())
                   .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
               ?? throw new TodosNotFound();
    }

    public async Task MarkAsCompletedAsync(TodoId id, MarkAsCompletedRequest request, CancellationToken cancellationToken = default)
    {
        var todo = await GetTodoAsync(id, cancellationToken);

        todo.IsCompleted = request.IsCompleted;
        todo.CompletedAt = request.IsCompleted ? DateTimeOffset.UtcNow : null;

        dbContext.Todos.Update(todo);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TodoDto> CreateAsync(UpsertTodoRequest request, CancellationToken cancellationToken = default)
    {
        var userId = httpContextAccessor.HttpContext!.User.GetTypedUserId()!.Value;

        var todo = Todo.Create(request.Title, request.Summery);

        var user = await dbContext.TodoUsers.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken: cancellationToken);
        if (user is null)
        {
            var authToken = httpContextAccessor.HttpContext!.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var appUser = await usersHttpApi.GetAsync(userId.Value, authToken, cancellationToken);
            user = TodoUser.Create(appUser);

            await dbContext.TodoUsers.AddAsync(user, cancellationToken);
        }

        todo.Creator = user;

        await dbContext.Todos.AddAsync(todo, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await GetAsync(todo.Id, cancellationToken);
    }

    public async Task UpdateAsync(TodoId id, UpsertTodoRequest request, CancellationToken cancellationToken = default)
    {
        var todo = await GetTodoAsync(id, cancellationToken);

        todo.Title = request.Title;
        todo.Summery = request.Summery;

        dbContext.Todos.Update(todo);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(TodoId id, CancellationToken cancellationToken = default)
    {
        var todo = await GetTodoAsync(id, cancellationToken);

        dbContext.Todos.Remove(todo);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}