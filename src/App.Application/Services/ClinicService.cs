using App.Application.Interfaces.Repos;
using App.Application.Interfaces.Services;
using App.Application.DTOs;
using App.Application.Common.Results;
using App.Application.Enums;

namespace App.Application.Services
{
  public class ClinicService(IClinicRepo repo) : IClinicService
  {
    private readonly IClinicRepo _repo = repo;
    public async Task<Result<GetClinicByNameResponse>> GetByNameAsync(string name, CancellationToken token = default)
    {
      // TODO: add logging, validation...etc
      var clinic = await _repo.GetByNameAsync(name, token);
      if(clinic is null)
        return Result<GetClinicByNameResponse>.Failure(new Error(ErrorType.NotFound, $"Clinic with name [{name}] was not found"));

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
