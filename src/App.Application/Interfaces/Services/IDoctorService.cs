using App.Application.Common.Results;
using App.Application.DTOs;

namespace App.Application.Interfaces.Services;
public interface IDoctorService
{
  public Task<Result<GetDoctorResponse>> GetByIdAsync(int id, CancellationToken token = default); 
  public Task<Result<IReadOnlyList<GetDoctorResponse>>> GetAllDoctorsAsync(Guid clinicId, CancellationToken token = default); 
  public Task<Result<GetDoctorResponse>> AddAsync(CreateDoctorRequest request, CancellationToken token = default); 
  public Task<Result> UpdateAsync(int id, UpdateDoctorRequest request, CancellationToken token = default); 
}
