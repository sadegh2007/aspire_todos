using AspireTodo.Core.Shared;

namespace AspireTodo.UserManagement.Events;

public record UserUpdatedTodosCount(UserId UserId, TodoId TodoId, int TodosCount);