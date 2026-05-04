using App.Application.DTOs;
using App.Application.Common.Results;
using App.Application.Common.Requests;
namespace App.Application.Interfaces.Services
{
  public interface IClinicService 
  {
    public Task<Result<GetClinicResponse>> GetByNameAsync(string name, CancellationToken token = default);
    public Task<Result<PagedResult<GetClinicResponse>>> GetAllAsync(GetPagedResultRequest request, CancellationToken token = default);
    public Task<Result> AddAsync(CreateClinicRequest request, CancellationToken token = default);
    public Task<Result> UpdateAsync(Guid id, UpdateClinicRequest request, CancellationToken token = default);
  }
}
