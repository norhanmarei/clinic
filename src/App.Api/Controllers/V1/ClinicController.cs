using App.Application.Interfaces.Services;
using App.Application.DTOs;
using App.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using App.Application.Common.Requests;
using App.Application.Common.Results;

namespace App.Api.Controllers.V1
{
  [ApiController]
  [Route("api/v1/clinics")]
  public class ClinicController(IClinicService service) : ControllerBase
  {
    private readonly IClinicService _service = service;

    [HttpGet("{name}", Name = "GetByNameAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetClinicResponse>> GetByNameAsync(string name, CancellationToken token = default)
    {
      var result = await _service.GetByNameAsync(name.Trim(), token);
      if(!result.IsSuccess)
        return result.ToActionResult(HttpContext);
      return new OkObjectResult(result.Value);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResult<GetClinicResponse>>> GetAllAsync([FromQuery] GetPagedResultRequest request, CancellationToken token = default)
    {
      var result = await _service.GetAllAsync(request, token);  
      if(!result.IsSuccess)
        return result.ToActionResult(HttpContext);
      return new OkObjectResult(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> AddAsync([FromBody] CreateClinicRequest request, CancellationToken token = default)
    {
      var result = await _service.AddAsync(request, token);  
      if(!result.IsSuccess)
        return result.ToActionResult(HttpContext);
      return CreatedAtRoute(
          "GetByNameAsync",
          new {name = request.Name.Trim()},
          null
          );
    }
  }
}
