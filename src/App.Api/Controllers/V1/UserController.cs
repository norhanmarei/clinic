using App.Api.Extensions;
using App.Application.DTOs;
using App.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers.V1; 
[ApiController]
[Route("api/v1/users")]
public class UserController(IUserService service): ControllerBase
{
  private readonly IUserService _service = service;

  [HttpGet("{id:int}", Name = "GetByIdAsync")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<GetUserResponse>> GetByIdAsync(int id, CancellationToken token =default)
  {
   var result = await _service.GetByIdAsync(id, token);
   if(!result.IsSuccess) 
     return result.ToActionResult(HttpContext);
   return Ok(result.Value);
  }

  [HttpGet("{username}", Name = "GetByUsernameAsync")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<GetUserResponse>> GetByUsernameAsync(string username, Guid clinicId, CancellationToken token =default)
  {
   var result = await _service.GetByUsernameAsync(username, clinicId, token);
   if(!result.IsSuccess) 
     return result.ToActionResult(HttpContext);
   return Ok(result.Value);
  }

  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<GetUserResponse>> GetAllUsersAsync(Guid clinicId, CancellationToken token =default)
  {
   var result = await _service.GetAllUsersAsync(clinicId, token);
   if(!result.IsSuccess) 
     return result.ToActionResult(HttpContext);
   return Ok(result.Value);
  }

  [HttpPut("id")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult> UpdateAsync(int id, [FromBody] UpdateUserRequest request, CancellationToken token = default)
  {
    var result = await _service.UpdateAsync(id, request, token);
    if(!result.IsSuccess) 
      return result.ToActionResult(HttpContext);
    return NoContent();
  }

  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status409Conflict)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<GetUserResponse>> AddAsync([FromBody] CreateUserRequest request, CancellationToken token =default)
  {
   var result = await _service.AddAsync(request, token);
   if(!result.IsSuccess) 
     return result.ToActionResult(HttpContext);
   return CreatedAtRoute(
       "GetByIdAsync",
       new {id = result.Value.Id},
       new {user = result.Value}
       );
  }
}
