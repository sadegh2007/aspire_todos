using AspireTodo.Core.Shared;
using AspireTodo.Todos.Shared;

namespace AspireTodo.UserManagement.Events;

public record FailedUserUpdateTodosCount(UserId UserId, TodoDto Todo, string? Message = "");