using App.Application.Interfaces.Repos;
using App.Application.Interfaces.Services;
using App.Application.DTOs;
using App.Application.Common.Results;
using App.Application.Enums;
using Microsoft.Extensions.Logging;
using App.Application.Common.Requests;
using App.Application.Mapping;
using App.Domain.Entities;
using App.Domain.ValueObjects;

namespace App.Application.Services
{
  public class ClinicService(ILogger<ClinicService> logger, IClinicRepo repo) : IClinicService
  {
    private readonly IClinicRepo _repo = repo;
    public async Task<Result<GetClinicResponse>> GetByNameAsync(string name, CancellationToken token = default)
    {
      if(name.Length > 200)
        return Result<GetClinicResponse>.Failure(new Error(ErrorType.BadRequest, $"Clinic name must not exceed 200 characters"));

      var clinic = await _repo.GetByNameAsync(name, token);
      if(clinic is null)
      {
        logger.LogInformation("clinic with name [{name}] was not found", name);
        return Result<GetClinicResponse>.Failure(new Error(ErrorType.NotFound, $"Clinic with name [{name}] was not found"));
      }

      var clinicResponse = Mappers.ToClinicResponse(clinic);
      return Result<GetClinicResponse>.Success(clinicResponse);
    }

    public async Task<Result<PagedResult<GetClinicResponse>>> GetAllAsync(GetPagedResultRequest request, CancellationToken token = default)
    {
      int offset = (request.Page -1) * request.PageSize;
      var list = await _repo.GetAllAsync(offset, request.PageSize, token);

      var clinicResponseList = Mappers.ToClinicResponse(list);
      var pagedResult = new PagedResult<GetClinicResponse> 
      {
        Page = request.Page,
        PageSize = request.PageSize,
        Data = clinicResponseList,
      };
      return Result<PagedResult<GetClinicResponse>>.Success(pagedResult);
    }

    public async Task<Result> AddAsync(CreateClinicRequest request, CancellationToken token = default)
    {
      string name = request.Name.Trim();
      if(await _repo.Exists(name, token))
      {
        logger.LogWarning("Conflict: clinic with name [{Name}] already exists", name);
        return Result.Failure(new Error(ErrorType.Conflict, $"Clininc with name [{name}] already exists"));
      }
      var clinic = Clinic.Create(name, new Timezone(request.Timezone), new WorkingHours(request.StartWorkingHour, request.EndWorkingHour));
      int affectedRows = await _repo.AddAsync(clinic, token);
      if(affectedRows == 0)
      {
        logger.LogWarning("Error: failed to create a new clinic with name [{Name}]", name);
        return Result.Failure(new Error(ErrorType.Unexpected, $"Failed to create clinic with name [{name}]"));
      }
      return Result.Success();
    }

    public async Task<Result> UpdateAsync(Guid id, UpdateClinicRequest request, CancellationToken token = default)
    {
      var clinic = await _repo.GetByIdAsync(id, token);
      if(clinic is null)
      {
        logger.LogWarning("Not Found: clinic with id [{Id}] was not found", id);
        return Result.Failure(new Error(ErrorType.NotFound, $"Clininc with id [{id}] was not found"));
      }

      string name = request.Name.Trim();
      var timezone = new Timezone(request.Timezone);
      var workingHours = new WorkingHours(request.Start, request.End);

      clinic.ChangeName(name);
      clinic.ChangeTimezone(timezone);
      clinic.ChangeWorkingHours(workingHours);
      if(request.IsActive && !clinic.IsActive)clinic.Activate();
      else if(!request.IsActive && clinic.IsActive)clinic.Deactivate();

      int affectedRows = await _repo.UpdateAsync(clinic, token);
      if(affectedRows == 0)
      {
        logger.LogWarning("Error: failed to update clinic with id [{Id}]", id);
        return Result.Failure(new Error(ErrorType.Unexpected, $"Failed to update clinic with id [{id}]"));
      }
      return Result.Success();
    }
  }
}
