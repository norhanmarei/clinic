using App.Application.Common.Results;
using App.Application.DTOs;
using App.Application.Interfaces.Repos;
using App.Application.Interfaces.Services;
using App.Application.Mapping;
using App.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace App.Application.Services;
public class PatientService(ILogger<PatientService> logger, IPatientRepo repo) : IPatientService
{
  private readonly ILogger<PatientService> _logger = logger;
  private readonly IPatientRepo _repo = repo;
  public async Task<Result<GetPatientResponse>> GetByIdAsync(int id, CancellationToken token = default)
  {
    if(id < 0)
    {
      _logger.LogInformation("Bad Request: patient id [{Id}] must be greater than or equal to 0", id);
      return Result<GetPatientResponse>.Failure(new Error(Enums.ErrorType.BadRequest, "Bad Request: patient id must be greater than or equal to 0"));
    }
    var patient = await _repo.GetByIdAsync(id, token);
    if(patient is null)
    {
      _logger.LogWarning("Not Found: patient with id [{Id}] was not found", id);
      return Result<GetPatientResponse>.Failure(new Error(Enums.ErrorType.NotFound, $"Not Found: patient with id [{id}] was not found"));
    }
    return Result<GetPatientResponse>.Success(Mappers.ToPatientResponse(patient));
  }

  public async Task<Result<IReadOnlyList<GetPatientResponse>>> GetPatientsPerClinicAsync(Guid clinicId, CancellationToken token = default)
  {
    if(clinicId == Guid.Empty)
    {
      _logger.LogInformation("Bad Request: clinic id [{Id}] is required", clinicId);
      return Result<IReadOnlyList<GetPatientResponse>>.Failure(new Error(Enums.ErrorType.BadRequest, $"Bad Request: clinic id [{clinicId}] is required"));
    }
    var list = await _repo.GetPatientsPerClinicAsync(clinicId, token);
    return Result<IReadOnlyList<GetPatientResponse>>.Success(Mappers.ToPatientResponse(list));
  }

  public async Task<Result<GetPatientResponse>> AddAsync(CreatePatientRequest request, CancellationToken token = default)
  {
    var newPatient = Patient.Create(request.FullName, request.BirthDate, request.Gender, request.Email, request.PhoneNumber, request.IsActive, request.ClinicId);
    int newPatientId = await _repo.AddAsync(newPatient, token);
    var dto = Mappers.ToPatientResponse(newPatientId, newPatient);
    return Result<GetPatientResponse>.Success(dto);
  }
  public async Task<Result> UpdateAsync(int id, UpdatePatientRequest request, CancellationToken token = default)
  {
    if(id < 0)
    {
      _logger.LogWarning("Bad Request: id [{id}] is invalid", id);
      return Result.Failure(new Error(Enums.ErrorType.BadRequest, $"Bad Request: id [{id}] is invalid"));
    }
    var patient = await _repo.GetByIdAsync(id, token);
    if(patient is null)
    {
      _logger.LogWarning("Not Found: patient with id [{id}] was not found", id);
      return Result.Failure(new Error(Enums.ErrorType.NotFound, $"Not Found: patient with id [{id}] was not found"));
    }

    patient.ChangeName(request.FullName);
    patient.ChangeGender(request.Gender);

    patient.ChangeEmail(request.Email);
    patient.ChangePhoneNumber(request.PhoneNumber);

    patient.ChangeBirthDate(request.BirthDate);
    patient.ChangeClinicId(request.ClinicId);

    await _repo.UpdateAsync(patient, token);
    return Result.Success();
  }
}
