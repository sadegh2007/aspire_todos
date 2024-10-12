using AspireTodo.Core.Shared;

namespace AspireTodo.UserManagement.Events;

public record FailedUserUpdateTodosCount(UserId UserId, TodoId TodoId);