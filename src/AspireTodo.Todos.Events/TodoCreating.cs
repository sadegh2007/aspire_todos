using AspireTodo.Core.Shared;

namespace AspireTodo.Todos.Events;

public record TodoCreating(string Title, string? Summery, UserId UserId, string AuthToken);