using App.Application.Interfaces.Services;
using App.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers.V1
{
  [ApiController]
  [Route("api/v1/clinics")]
  public class ClinicController(IClinicService service) : ControllerBase
  {
    private readonly IClinicService _service = service;

    [HttpGet("{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
      public async Task<ActionResult<Clinic?>> GetByNameAsync(string name, CancellationToken token = default)
      {
        var clinic = await _service.GetByNameAsync(name, token);
        if(clinic is null) return NotFound($"Clinic with name {name} was not found");
        return Ok(clinic);
      }
  }
}
