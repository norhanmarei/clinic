using App.Domain.Entities;

namespace App.Application.Interfaces.Repos;
public interface IDoctorRepo
{
  public Task<Doctor?> GetByIdAsync(int id, CancellationToken token = default);
  // public Task<IReadOnlyList<Doctor>> GetByNameAsync(string name, Guid clinicId, CancellationToken token = default);
  public Task<IReadOnlyList<Doctor>> GetAllDoctorsAsync(Guid clinicId, CancellationToken token = default);
  public Task<bool> Exists(int id, CancellationToken token = default);
  /// <Returns> The doctor id for new doctor (generated on database)
  public Task<int> AddAsync(Doctor doctor, CancellationToken token = default);
  /// <Returns> The number of rows affected by the update
  public Task<int> UpdateAsync(Doctor doctor, CancellationToken token = default);
}
