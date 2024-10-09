using AspireTodo.Core.Exceptions;

namespace AspireTodo.UserManagement.Exceptions;

public class UserNotFound(string message = "User not found."): NotFoundException(message);