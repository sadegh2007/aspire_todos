using AspireTodo.Core.Identity;
using AspireTodo.Core.Shared;
using AspireTodo.Todos.Events;
using AspireTodo.Todos.Grpc.Data;
using Google.Protobuf.Collections;
using Gridify;
using Gridify.EntityFramework;
using Grpc.Core;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AspireTodo.Todos.Grpc.GrpcServices;

public class TodoService(
    TodosDbContext appDbContext,
    IPublishEndpoint publishEndpoint) : TodoIt.TodoItBase
{
    public override async Task<TodoListResponse> List(TodoListRequest request, ServerCallContext context)
    {
        var userId = context.GetHttpContext().User.GetTypedUserId();

        var todos = await appDbContext.Todos.AsNoTracking()
            .Where(x => x.Creator.UserId == userId)
            .GridifyAsync(new GridifyQuery(request.Page, request.PageSize, ""), context.CancellationToken);

        var list = new RepeatedField<TodoGetResponse>();
        list.AddRange(todos.Data.Select(todo => new TodoGetResponse
        {
            Title = todo.Title,
            Summery = todo.Summery,
            CreatedAt = todo.CreatedAt.ToString("yyyy-M-d HH:mm:ss"),
            IsCompleted = todo.IsCompleted
        }));

        var response = new TodoListResponse();
        response.Count = list.Count;
        response.Data.AddRange(todos.Data.Select(todo => new TodoGetResponse
        {
            Title = todo.Title,
            Summery = todo.Summery,
            CreatedAt = todo.CreatedAt.ToString("yyyy-M-d HH:mm:ss"),
            IsCompleted = todo.IsCompleted
        }));

        return response;
    }

    public override async Task<TodoGetResponse> Get(TodoGetRequest request, ServerCallContext context)
    {
        var todo = await appDbContext.Todos.FirstOrDefaultAsync(x => x.Id == TodoId.FromInt32(request.Id));
        if (todo == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Todo not found!"));
        }

        return new TodoGetResponse
        {
            Title = todo.Title,
            Summery = todo.Summery,
            CreatedAt = todo.CreatedAt.ToString("yyyy-M-d HH:mm:ss"),
            IsCompleted = todo.IsCompleted
        };
    }

    public override async Task<TodoCreateResponse> CreateTodo(TodoCreateRequest request, ServerCallContext context)
    {
        var userId = context.GetHttpContext().User.GetTypedUserId()!.Value;
        var authToken = context.GetHttpContext().Request.Headers["Authorization"].ToString()
            .Replace("Bearer ", "");

        await publishEndpoint.Publish<TodoCreating>(new(request.Title, request.Summery, userId, authToken),
            context.CancellationToken);

        return new TodoCreateResponse();
    }

    public override async Task<TodoUpdateResponse> Update(TodoUpdateRequest request, ServerCallContext context)
    {
        var todo = await appDbContext.Todos.FirstOrDefaultAsync(x => x.Id == TodoId.FromInt32(request.Id));
        if (todo == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Todo not found!"));
        }

        todo.Title = request.Title;
        todo.Summery = request.Summery;

        appDbContext.Todos.Update(todo);
        await appDbContext.SaveChangesAsync(context.CancellationToken);

        return new TodoUpdateResponse();
    }

    public override async Task<TodoRemoveResponse> Remove(TodoRemoveRequest request, ServerCallContext context)
    {
        var userId = context.GetHttpContext().User.GetTypedUserId()!.Value;
        await publishEndpoint.Publish<TodoRemoving>(new(TodoId.FromInt32(request.Id), userId), context.CancellationToken);

        return new TodoRemoveResponse();
    }
}