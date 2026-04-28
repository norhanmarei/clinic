using App.Application.Common.Results;
using App.Application.Enums;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Extensions;
public static class ResultExtensions
{
  public static ActionResult ToActionResult<T>(this Result<T> result, HttpContext httpContext)
  {
    // TODO: handle different success scenarios
    if(result.IsSuccess)
      return new OkObjectResult(result.Value);

    string title = result.Error!.Type switch 
    {
      ErrorType.BadRequest => "Bad Request", 
      ErrorType.NotFound => "Not Found", 
      ErrorType.Conflict => "Conflict", 
      ErrorType.Unauthorized => "Forbidden", 
      ErrorType.Unauthenticated => "Unauthenticated", 
      _ => "Unexpected Error", 
    };

    int status = result.Error.Type switch 
    {
      ErrorType.BadRequest => StatusCodes.Status400BadRequest, 
      ErrorType.NotFound => StatusCodes.Status404NotFound, 
      ErrorType.Conflict => StatusCodes.Status409Conflict, 
      ErrorType.Unauthenticated => StatusCodes.Status401Unauthorized, 
      ErrorType.Unauthorized => StatusCodes.Status403Forbidden, 
      _ => StatusCodes.Status500InternalServerError,  
    };

 
    var problemDetails= new ProblemDetails 
    {
      Title = title, 
      Status = status, 
      Detail = result.Error.Message,
      Instance = httpContext.Request.Path
    };

    problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
    problemDetails.Extensions["timestamp"] = DateTime.UtcNow;

    return new ObjectResult(problemDetails){StatusCode = status};
  }
}
