using System.Net;

namespace AspireTodo.Core.Exceptions;

public class BadRequestException(string message, string title = "Bad Request") : HttpException(message, title, HttpStatusCode.BadRequest);
