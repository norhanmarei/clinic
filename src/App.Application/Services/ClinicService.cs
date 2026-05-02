using App.Application.Interfaces.Repos;
using App.Application.Interfaces.Services;
using App.Application.DTOs;
using App.Application.Common.Results;
using App.Application.Enums;
using Microsoft.Extensions.Logging;
using App.Application.Common.Requests;
using App.Application.Mapping;

namespace App.Application.Services
{
  public class ClinicService(ILogger<ClinicService> logger, IClinicRepo repo) : IClinicService
  {
    private readonly IClinicRepo _repo = repo;
    public async Task<Result<GetClinicResponse>> GetByNameAsync(string name, CancellationToken token = default)
    {
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
  }
}
