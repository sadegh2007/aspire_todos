using AspireTodo.Core.Identity;
using AspireTodo.Core.Shared;
using AspireTodo.Todos.Data;
using AspireTodo.Todos.Domain;
using AspireTodo.Todos.Events;
using AspireTodo.Todos.Exceptions;
using AspireTodo.Todos.Features.Todos.Data.Mappers;
using AspireTodo.Todos.Shared;
using Gridify;
using Gridify.EntityFramework;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AspireTodo.Todos.Features.Todos.Services;

public class TodoService(
    TodosDbContext dbContext,
    IHttpContextAccessor httpContextAccessor,
    IPublishEndpoint publishEndpoint
) : ITodoService
{
    public async Task<Paging<TodoDto>> ListAsync(GridifyQuery query, CancellationToken cancellationToken = default)
    {
        var userId = httpContextAccessor.HttpContext!.User.GetTypedUserId()!.Value;

        return await dbContext.Todos.AsNoTracking()
            .Where(x => x.Creator.UserId == userId)
            .Select(x => x.ToDto())
            .OrderByDescending(x => x.CreatedAt)
            .GridifyAsync(query, cancellationToken);
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

    public async Task MarkAsCompletedAsync(TodoId id, MarkAsCompletedRequest request,
        CancellationToken cancellationToken = default)
    {
        var todo = await GetTodoAsync(id, cancellationToken);

        todo.IsCompleted = request.IsCompleted;
        todo.CompletedAt = request.IsCompleted ? DateTimeOffset.UtcNow : null;

        dbContext.Todos.Update(todo);
        await dbContext.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish<TodoCompleted>(new TodoCompleted(id, todo.IsCompleted), cancellationToken);
    }

    public async Task CreateAsync(UpsertTodoRequest request, CancellationToken cancellationToken = default)
    {
        var userId = httpContextAccessor.HttpContext!.User.GetTypedUserId()!.Value;
        var authToken = httpContextAccessor.HttpContext!.Request.Headers["Authorization"].ToString()
            .Replace("Bearer ", "");

        await publishEndpoint.Publish<TodoCreating>(new TodoCreating(request.Title, request.Summery, userId, authToken),
            cancellationToken);
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
        var userId = httpContextAccessor.HttpContext!.User.GetTypedUserId()!.Value;
        await publishEndpoint.Publish<TodoRemoving>(new TodoRemoving(id, userId), cancellationToken);
    }

    private async Task<Todo> GetTodoAsync(TodoId id, CancellationToken cancellationToken = default)
    {
        var userId = httpContextAccessor.HttpContext!.User.GetTypedUserId();
        return await dbContext.Todos.AsNoTracking()
                   .Where(x => x.Creator.UserId == userId)
                   .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
               ?? throw new TodosNotFound();
    }
}