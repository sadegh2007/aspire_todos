using AspireTodo.Core.Shared;

namespace AspireTodo.Todos.Events;

public record TodoRemoving(TodoId TodoId, UserId UserId);