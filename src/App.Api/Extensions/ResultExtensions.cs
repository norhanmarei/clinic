using App.Application.Common.Results;
using App.Application.Enums;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Extensions;
public static class ResultExtensions
{
  private static ObjectResult _ToProblemDetails(
      Error error,
      HttpContext httpContext)
  {
    var title = error.Type switch
    {
      ErrorType.BadRequest => "Bad Request",
      ErrorType.NotFound => "Not Found",
      ErrorType.Conflict => "Conflict",
      ErrorType.Unauthorized => "Forbidden",
      ErrorType.Unauthenticated => "Unauthenticated",
      _ => "Unexpected Error",
    };

    var status = error.Type switch
    {
      ErrorType.BadRequest => StatusCodes.Status400BadRequest,
      ErrorType.NotFound => StatusCodes.Status404NotFound,
      ErrorType.Conflict => StatusCodes.Status409Conflict,
      ErrorType.Unauthenticated => StatusCodes.Status401Unauthorized,
      ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
      _ => StatusCodes.Status500InternalServerError,
    };

    var problemDetails = new ProblemDetails
    {
      Title = title,
      Status = status,
      Detail = error.Message,
      Instance = httpContext.Request.Path
    };

    problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
    problemDetails.Extensions["timestamp"] = DateTime.UtcNow;

    return new ObjectResult(problemDetails)
    {
      StatusCode = status
    };
  }
  public static ActionResult ToActionResult<T>(this Result<T> result, HttpContext httpContext)
  {
    return _ToProblemDetails(result.Error!, httpContext);
  }

  public static ActionResult ToActionResult(this Result result, HttpContext httpContext)
  {
    return _ToProblemDetails(result.Error!, httpContext);
  }
}
