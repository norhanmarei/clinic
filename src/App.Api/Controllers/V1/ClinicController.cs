using App.Application.Interfaces.Services;
using App.Application.DTOs;
using App.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers.V1
{
  [ApiController]
  [Route("api/v1/clinics")]
  public class ClinicController(IClinicService service) : ControllerBase
  {
    private readonly IClinicService _service = service;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetClinicByNameResponse>> GetByNameAsync([FromQuery] GetClinicByNameRequest request, CancellationToken token = default)
    {
      var result = await _service.GetByNameAsync(request.Name, token);
      return result.ToActionResult<GetClinicByNameResponse>();
    }
  }
}
