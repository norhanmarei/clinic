using App.Api.Extensions;
using App.Application.DTOs;
using App.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers.V1;
[ApiController]
[Route("api/v1/doctors")]
public class DoctorController(IDoctorService service): ControllerBase
{
  private readonly IDoctorService _service = service;

  [HttpGet("{id:int}", Name = "GetDoctorByIdAsync")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<GetDoctorResponse>> GetByIdAsync(int id, CancellationToken token =default)
  {
   var result = await _service.GetByIdAsync(id, token);
   if(!result.IsSuccess)
     return result.ToActionResult(HttpContext);
   return Ok(result.Value);
  }

  [HttpGet("{clinicId}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<GetDoctorResponse>> GetAllDoctorsAsync(Guid clinicId, CancellationToken token =default)
  {
   var result = await _service.GetAllDoctorsAsync(clinicId, token);
   if(!result.IsSuccess)
     return result.ToActionResult(HttpContext);
   return Ok(result.Value);
  }

  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<GetDoctorResponse>> AddAsync([FromBody] CreateDoctorRequest request, CancellationToken token =default)
  {
   var result = await _service.AddAsync(request, token);
   if(!result.IsSuccess)
     return result.ToActionResult(HttpContext);
   return CreatedAtRoute(
       "GetDoctorByIdAsync",
       new {id = result.Value.Id},
       result.Value
       );
  }

  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult> UpdateAsync(int id, [FromBody] UpdateDoctorRequest request, CancellationToken token =default)
  {
   var result = await _service.UpdateAsync(id, request, token);
   if(!result.IsSuccess)
     return result.ToActionResult(HttpContext);
   return NoContent();
  }
}
