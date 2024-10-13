using AspireTodo.Core.Shared;
using AspireTodo.Todos.Shared;

namespace AspireTodo.Todos.Events;

public record TodoCreated(TodoDto Todo, UserId UserId, int UserTodosCount);