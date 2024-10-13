using AspireTodo.Core.Shared;

namespace AspireTodo.Todos.Events;

public record TodoNotFound(TodoId TodoId, UserId UserId);