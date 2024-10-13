using AspireTodo.Core.Shared;
using AspireTodo.Todos.Shared;

namespace AspireTodo.UserManagement.Events;

public record UserUpdatedTodosCount(UserId UserId, TodoDto Todo, int TodosCount);