using App.Application.Common.Results;
using App.Application.Enums;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Extensions;
public static class ResultExtensions
{
  public static ActionResult ToActionResult<T>(this Result<T> result)
  {
    // TODO: handle different success scenarios
    if(result.IsSuccess)
      return new OkObjectResult(result.Value);
    return result.Error.Type switch 
    {
      ErrorType.BadRequest => new BadRequestObjectResult(result.Error.Message),
      ErrorType.NotFound => new NotFoundObjectResult(result.Error.Message),
      ErrorType.Conflict => new ConflictObjectResult(result.Error.Message),
      ErrorType.Unauthorized => new UnauthorizedResult(),
      _ => new ObjectResult(result.Error.Message)
      {
        StatusCode = StatusCodes.Status500InternalServerError
      }
    };
  }
}
