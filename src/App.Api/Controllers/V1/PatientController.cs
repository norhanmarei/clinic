using App.Api.Extensions;
using App.Application.DTOs;
using App.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers.V1;
[ApiController]
[Route("api/v1/patients")]
public class PatientController(IPatientService service) : ControllerBase
{
  private readonly IPatientService _service = service;
  
  [HttpGet("{id:int}", Name = "GetPatientByIdAsync")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<GetPatientResponse>> GetByIdAsync(int id, CancellationToken token = default)
  {
    var result = await _service.GetByIdAsync(id, token);
    if(!result.IsSuccess)
     return result.ToActionResult(HttpContext);
    return Ok(result.Value);
  }

  [HttpGet("clinic/{clinicId:guid}", Name = "GetPatientsByClinicIdAsync")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<IReadOnlyList<GetPatientResponse>>> GetPatientsPerClinicAsync(Guid clinicId, CancellationToken token = default)
  {
    var result = await _service.GetPatientsPerClinicAsync(clinicId, token);
    if(!result.IsSuccess)
     return result.ToActionResult(HttpContext);
    return Ok(result.Value);
  }

  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status409Conflict)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<GetPatientResponse>> AddAsync([FromBody]CreatePatientRequest req, CancellationToken token = default)
  {
    var result = await _service.AddAsync(req, token);
    if(!result.IsSuccess)
     return result.ToActionResult(HttpContext);
    return CreatedAtRoute
        (
          "GetPatientByIdAsync",
          new {Id = result.Value.Id},
          result.Value
        );
  }

  [HttpPut("{id:int}")]
  [ProducesResponseType(StatusCodes.Status409Conflict)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult> UpdateAsync(int id, [FromBody]UpdatePatientRequest req, CancellationToken token = default)
  {
    var result = await _service.UpdateAsync(id, req, token);
    if(!result.IsSuccess)
     return result.ToActionResult(HttpContext);
    return NoContent();
  }

}
