using AspireTodo.Core.Shared;

namespace AspireTodo.UserManagement.Events;

public record UserUpdated(UserId UserId, string? Name, string? Family);