using AspireTodo.Core.Exceptions;

namespace AspireTodo.Todos.Exceptions;

public class TodosNotFound(string message = "Todos not found."): NotFoundException(message);