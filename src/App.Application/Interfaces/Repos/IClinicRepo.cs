using App.Domain.Entities;
namespace App.Application.Interfaces.Repos
{
  public interface IClinicRepo
  {
    public Task<Clinic?> GetByNameAsync(string name, CancellationToken token = default); 
    public Task<IReadOnlyList<Clinic>> GetAllAsync(int offset, int limit, CancellationToken token = default); 
  }
}
