using AspireTodo.Todos.Shared;
using Gridify;
using Refit;

namespace AspireTodo.Todos.HttpClient;

public interface ITodosHttpApi
{
    [Get("/api/todos")]
    public Task<Paging<TodoDto>> ListAsync([Query] GridifyQuery query, [Authorize] string token);

    [Get("/api/todos/{id}")]
    public Task<TodoDto> GetAsync(int id, [Authorize] string token);
}