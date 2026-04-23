using App.Domain.Entities;

namespace App.Application.Interfaces.Services
{
  public interface IClinicService 
  {
    public Task<Clinic?> GetByNameAsync(string name, CancellationToken token = default);
  }
}
