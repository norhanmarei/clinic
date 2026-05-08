using App.Application.Common.Results;
using App.Application.DTOs;

namespace App.Application.Interfaces.Services;
public interface IPatientService
{
  public Task<Result<GetPatientResponse>> GetByIdAsync(int id, CancellationToken token = default); 
  public Task<Result<IReadOnlyList<GetPatientResponse>>> GetPatientsPerClinicAsync(Guid clinicId, CancellationToken token = default); 
  public Task<Result<GetPatientResponse>> AddAsync(CreatePatientRequest request, CancellationToken token = default); 
  public Task<Result> UpdateAsync(int id, UpdatePatientRequest request, CancellationToken token = default); 
}
