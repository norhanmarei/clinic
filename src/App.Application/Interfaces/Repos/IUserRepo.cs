using App.Domain.Entities;
namespace App.Application.Interfaces.Repos;
public interface IUserRepo
{
  public Task<User?> GetByIdAsync(int id, CancellationToken token = default);
  public Task<User?> GetByUsernameAsync(string username, Guid clinicId, CancellationToken token = default);
  public Task<IReadOnlyList<User>> GetAllUsersAsync(Guid clinicId, CancellationToken token = default);
  public Task<bool> Exists(string username, Guid clinicId, CancellationToken token = default);
  // public Task<bool> Exists(int id, CancellationToken token = default);
  /// <Returns> The user id for new user (generated on database)
  public Task<int> AddAsync(User user, CancellationToken token = default);
  /// <Returns> The number of rows affected by the update
  public Task<int> UpdateAsync(User user, CancellationToken token = default);
}
