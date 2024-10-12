using AspireTodo.Core.Shared;

namespace AspireTodo.Todos.Events;

public record TodoCreated(TodoId TodoId, UserId UserId, int UserTodosCount);