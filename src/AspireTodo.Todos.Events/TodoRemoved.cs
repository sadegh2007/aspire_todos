using AspireTodo.Core.Shared;

namespace AspireTodo.Todos.Events;

public record TodoRemoved(TodoId TodoId, UserId UserId);