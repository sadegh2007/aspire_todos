namespace AspireTodo.UserManagement.Exceptions;

public class IncorrectUserPasswordException(string message = "PhoneNumber or password is incorrect."): UnauthorizedAccessException(message);