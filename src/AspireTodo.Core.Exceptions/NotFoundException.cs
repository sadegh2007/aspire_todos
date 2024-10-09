using System.Net;

namespace AspireTodo.Core.Exceptions;

public class NotFoundException(string message, string title = "Not Found") : HttpException(message, title, HttpStatusCode.NotFound);
