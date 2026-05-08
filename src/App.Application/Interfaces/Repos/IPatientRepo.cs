using App.Domain.Entities;

namespace App.Application.Interfaces.Repos;
public interface IPatientRepo
{
  public Task<Patient?> GetByIdAsync(int id, CancellationToken token = default);
  // TODO
  // public Task<IReadOnlyList<Patient>> GetByNameAsync(string name, Guid clinicId, CancellationToken token = default);
  // public Task<Patient?> GetByNameAsync(string name, CancellationToken token = default);
  // public Task<IReadOnlyList<Patient>> GetByNameAsync(string name, Guid clinicId, CancellationToken token = default);
  public Task<IReadOnlyList<Patient>> GetPatientsPerClinicAsync(Guid clinicId, CancellationToken token = default);
  // public Task<IReadOnlyList<Patient>> GetPatientsPerDoctorAsync(int id, CancellationToken token = default);
  public Task<bool> Exists(int id, CancellationToken token = default);
  /// <Returns> The Patient id for new Patient (generated on database)
  public Task<int> AddAsync(Patient patient, CancellationToken token = default);
  /// <Returns> The number of rows affected by the update
  public Task<int> UpdateAsync(Patient patient, CancellationToken token = default);
}
