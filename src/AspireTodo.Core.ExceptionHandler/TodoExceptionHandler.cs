using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Refit;
using AspireTodo.Core.Exceptions;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace AspireTodo.Core.ExceptionHandler;

public partial class TodoExceptionHandler(ILogger<TodoExceptionHandler> logger) : IExceptionHandler
{
	private readonly ILogger<TodoExceptionHandler> _logger = logger;

	[LoggerMessage(Level = LogLevel.Error)]
	partial void LogError(Exception occurred);

	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		var problemDetails = exception switch
		{
			HttpException httpException => ProcessHttpException(httpException),
			ApiException apiException => await ProcessApiException(apiException),
			_ => ProcessUnKnownException(exception)
		};

		httpContext.Response.StatusCode = problemDetails.Status ?? 400;

		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

		return true;
	}
	private static async Task<ProblemDetails> ProcessApiException(ApiException exception)
	{
		var content = await exception.GetContentAsAsync<ProblemDetails>();

		return content ?? new ProblemDetails
		{
			Status = (int)exception.StatusCode,
			Title = exception.ReasonPhrase,
			Detail = exception.Message
		};
	}

	private static ProblemDetails ProcessHttpException(HttpException exception)
	{
		var problemDetails = new ProblemDetails
		{
			Status = (int)exception.StatusCode,
			Title = exception.Title,
			Detail = exception.Message
		};

		return problemDetails;
	}

	private ProblemDetails ProcessUnKnownException(Exception exception)
	{
		LogError(exception);

		var problemDetails = new ProblemDetails
		{
			Status = StatusCodes.Status400BadRequest,
			Title = "Bad Request",
			Detail = exception.Message
		};

		return problemDetails;
	}
}
