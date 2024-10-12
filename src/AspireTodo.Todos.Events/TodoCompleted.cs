using AspireTodo.Core.Shared;

namespace AspireTodo.Todos.Events;

public record TodoCompleted(TodoId TodoId, bool IsCompleted);