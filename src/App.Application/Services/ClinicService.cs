using App.Application.Interfaces.Repos;
using App.Application.Interfaces.Services;
using App.Application.DTOs;
using App.Application.Common.Results;
using App.Application.Enums;
using Microsoft.Extensions.Logging;

namespace App.Application.Services
{
  public class ClinicService(ILogger<ClinicService> logger, IClinicRepo repo) : IClinicService
  {
    private readonly IClinicRepo _repo = repo;
    public async Task<Result<GetClinicByNameResponse>> GetByNameAsync(string name, CancellationToken token = default)
    {
      var clinic = await _repo.GetByNameAsync(name, token);
      if(clinic is null)
      {
        logger.LogInformation("clinic with name [{name}] was not found", name);
        return Result<GetClinicByNameResponse>.Failure(new Error(ErrorType.NotFound, $"Clinic with name [{name}] was not found"));
      }

      return Result<GetClinicByNameResponse>.Success(new GetClinicByNameResponse
          {
          Id = clinic.Id, 
          Name = clinic.Name,
          Timezone = clinic.Timezone,
          IsActive = clinic.IsActive,
          WorkingHours = clinic.WorkingHours,
          CreatedAt = clinic.CreatedAt,
          UpdatedAt = clinic.UpdatedAt
          });
    }
  }
}
