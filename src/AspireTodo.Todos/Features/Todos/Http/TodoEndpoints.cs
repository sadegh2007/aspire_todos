using Gridify;
using AspireTodo.Core.Shared;
using AspireTodo.Todos.Shared;
using Microsoft.AspNetCore.Mvc;
using AspireTodo.Todos.Features.Todos.Services;

namespace AspireTodo.Todos.Features.Todos.Http;

public static class TodoEndpoints
{
    public static IEndpointRouteBuilder MapTodos(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("todos");

        api.MapGet("", List);
        api.MapPost("", Create);
        api.MapGet("{id:int}", Get);
        api.MapPut("{id:int}/completed", Completed);
        api.MapPut("{id:int}", Update);
        api.MapDelete("{id:int}", Remove);

        return api;
    }

    private static async Task<Paging<TodoDto>> List([AsParameters] GridifyQuery query, ITodoService todoService)
        => await todoService.ListAsync(query);

    private static async Task<TodoDto> Get(TodoId id, ITodoService todoService)
        => await todoService.GetAsync(id);

    private static async Task Completed(TodoId id, ITodoService todoService)
        => await todoService.MarkAsCompletedAsync(id);

    private static async Task<TodoDto> Create([FromBody] UpsertTodoRequest request, ITodoService todoService)
        => await todoService.CreateAsync(request);

    private static async Task Update(TodoId id, [FromBody] UpsertTodoRequest request, ITodoService todoService)
        => await todoService.UpdateAsync(id, request);
    private static async Task Remove(TodoId id, ITodoService todoService)
        => await todoService.RemoveAsync(id);
}