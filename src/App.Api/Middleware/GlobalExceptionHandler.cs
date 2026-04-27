using System.Data.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Middleware;
public sealed class GlobalExceptionHandler
(ILogger<GlobalExceptionHandler> logger, IProblemDetailsService problemDetailsService) : IExceptionHandler
{
  private static string GetSafeErrorMessage(HttpContext httpContext, Exception exception)
  {
    var env = httpContext.RequestServices.GetRequiredService<IHostEnvironment>();
    return env.IsDevelopment() ? exception.Message : "Unhandled exception occurred.";
  }
  private static (string Title, int Status) MapException(Exception exception) 
    => exception switch
    {
      ArgumentException => ("Invalid argument", StatusCodes.Status400BadRequest),
      UnauthorizedAccessException => ("Unauthorized access", StatusCodes.Status401Unauthorized),
      DbException => ("An unexpected Db error occurred", StatusCodes.Status500InternalServerError),
      _ => ("An unexpected error occurred.", StatusCodes.Status500InternalServerError),
    };
  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
  {
    logger.LogError(exception,
                    "Unhandled exception occurred. TraceId: {TraceId}, Path: {Path}", 
                    httpContext.TraceIdentifier,
                    httpContext.Request.Path);
    var (title, status) = MapException(exception);
    httpContext.Response.StatusCode = status; 
    var problemDetails = new ProblemDetails
    {
      Status = status,
      Instance = httpContext.Request.Path,
      Title = title,
      Detail = GetSafeErrorMessage(httpContext, exception)
    };
    problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
    problemDetails.Extensions["timestamp"] = DateTime.UtcNow;
    return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext 
        {
        HttpContext = httpContext,
        ProblemDetails = problemDetails
        });
  }
}
