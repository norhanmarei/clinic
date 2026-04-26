using App.Application.DTOs;
using App.Application.Common.Results;
namespace App.Application.Interfaces.Services
{
  public interface IClinicService 
  {
    public Task<Result<GetClinicByNameResponse>> GetByNameAsync(string name, CancellationToken token = default);
  }
}
