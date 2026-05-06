using App.Application.Common.Results;
using App.Application.DTOs;
using App.Application.Interfaces.Repos;
using App.Application.Interfaces.Services;
using App.Application.Mapping;
using App.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace App.Application.Services;
public class DoctorService(ILogger<DoctorService> logger, IDoctorRepo repo): IDoctorService
{
  private readonly ILogger _logger = logger;
  private readonly IDoctorRepo _repo = repo;
 public async Task<Result<GetDoctorResponse>> GetByIdAsync(int id, CancellationToken token = default)
 {
   if(id < 0)
   {
     _logger.LogInformation("Doctor id [{Id}] must be greater than or equal to 0", id);
     return Result<GetDoctorResponse>.Failure(new Error(Enums.ErrorType.BadRequest, "Bad Request: doctor id must be greater than or equal to 0"));
   }
   var doctor = await _repo.GetByIdAsync(id, token);
   if(doctor is null)
   {
     _logger.LogInformation("Not Found: Doctor with id [{Id}] was not found", id);
    return Result<GetDoctorResponse>.Failure(new Error(Enums.ErrorType.NotFound, $"Not Found: Doctor with id [{id}] was not found"));
   }

   return Result<GetDoctorResponse>.Success(Mappers.ToDoctorResponse(doctor));
 }
 public async Task<Result<IReadOnlyList<GetDoctorResponse>>> GetAllDoctorsAsync(Guid clinicId, CancellationToken token = default)
 {
   if(clinicId == Guid.Empty)
   {
     _logger.LogInformation("Clinic id [{Id}] is required", clinicId);
     return Result<IReadOnlyList<GetDoctorResponse>>.Failure(new Error(Enums.ErrorType.BadRequest, "Bad Request: clinic id is required"));
   }
   var list = await _repo.GetAllDoctorsAsync(clinicId, token);
   return Result<IReadOnlyList<GetDoctorResponse>>.Success(Mappers.ToDoctorResponse(list));
 }

 public async Task<Result<GetDoctorResponse>> AddAsync(CreateDoctorRequest request, CancellationToken token = default) 
 {
   Doctor doctor = Doctor.Create(request.FullName.Trim(), request.Specialty.Trim(), request.Gender.Trim(), request.IsActive, request.UserId, request.ClinicId);
   int newId = await _repo.AddAsync(doctor, token);
   var dto = new GetDoctorResponse 
        {
        Id = newId, 
        FullName = doctor.FullName,
        Specialty = doctor.Specialty,
        Gender = doctor.Gender,
        IsActive = doctor.IsActive,
        UserId = doctor.UserId,
        ClinicId = doctor.ClinicId,
        CreatedAt = doctor.CreatedAt,
        UpdatedAt = doctor.UpdatedAt
        };
   return Result<GetDoctorResponse>.Success(dto);
 }
 public async Task<Result> UpdateAsync(int id, UpdateDoctorRequest request, CancellationToken token = default)
 {
    if(id < 0) 
    {
      _logger.LogInformation("Doctor id [{Id}] must be greater than or equal to 0", id);
      return Result.Failure(new Error(Enums.ErrorType.BadRequest, "Bad Request: Doctor id must be greater than or equal to 0"));
    }
    var doctor = await _repo.GetByIdAsync(id, token);
    if(doctor is null)
    {
      _logger.LogWarning("Doctor with id [{Id}] was not found", id);
      return Result.Failure(new Error(Enums.ErrorType.NotFound, $"Doctor with id [{id}] was not found"));
    }
    doctor.ChangeName(request.FullName.Trim());
    doctor.ChangeSpecialty(request.Specialty.Trim());
    doctor.ChangeGender(request.Gender.Trim());
    doctor.ChangeUserId(request.UserId);
    doctor.ChangeClinicId(request.ClinicId);

    if(request.IsActive && !doctor.IsActive) doctor.Activate();
    if(!request.IsActive && doctor.IsActive) doctor.Deactivate();

    int affectedRows = await _repo.UpdateAsync(doctor, token);
    if(affectedRows < 1)
    {
      _logger.LogWarning("Unexpected: failed to update doctor with [{Id}]", id);
      return Result.Failure(new Error(Enums.ErrorType.Unexpected, $"Unexpected: failed to update doctor with [{id}]"));
    }
    return Result.Success();
  }
}
