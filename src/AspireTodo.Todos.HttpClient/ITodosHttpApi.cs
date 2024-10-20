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

    [Post("/api/todos")]
    public Task CreateAsync([Body] UpsertTodoRequest request, [Authorize] string token);

    [Put("/api/todos/{id}")]
    public Task UpdateAsync(int id, [Body] UpsertTodoRequest request, [Authorize] string token);

    [Delete("/api/todos/{id}")]
    public Task RemoveAsync(int id, [Authorize] string token);
    
    [Put("/api/todos/{id}/completed")]
    public Task MarkCompletedAsync(int id, [Body] MarkAsCompletedRequest request, [Authorize] string token);
}