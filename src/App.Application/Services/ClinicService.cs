using App.Application.Interfaces.Repos;
using App.Application.Interfaces.Services;
using App.Domain.Entities;

namespace App.Application.Services
{
  public class ClinicService(IClinicRepo repo) : IClinicService
  {
    private readonly IClinicRepo _repo = repo;
    public async Task<Clinic?> GetByNameAsync(string name, CancellationToken token = default)
    {
      // TODO: add logging, validation...etc
      return await _repo.GetByNameAsync(name, token);
    }
  }
}
