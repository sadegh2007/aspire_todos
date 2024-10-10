using AspireTodo.Core.Shared;
using AspireTodo.Todos.Shared;
using Gridify;

namespace AspireTodo.Todos.Features.Todos.Services;

public interface ITodoService
{
    public Task<Paging<TodoDto>> ListAsync(GridifyQuery query, CancellationToken cancellationToken = default);
    public Task<TodoDto> GetAsync(TodoId id, CancellationToken cancellationToken = default);
    public Task MarkAsCompletedAsync(TodoId id, MarkAsCompletedRequest request, CancellationToken cancellationToken = default);
    public Task<TodoDto> CreateAsync(UpsertTodoRequest request, CancellationToken cancellationToken = default);
    public Task UpdateAsync(TodoId id, UpsertTodoRequest request, CancellationToken cancellationToken = default);
    public Task RemoveAsync(TodoId id, CancellationToken cancellationToken = default);
}