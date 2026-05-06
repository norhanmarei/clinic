using App.Application.Common.Results;
using App.Application.DTOs;

namespace App.Application.Interfaces.Services;
public interface IUserService
{
  public Task<Result<GetUserResponse>> GetByIdAsync(int id, CancellationToken token = default);
  public Task<Result<GetUserResponse>> GetByUsernameAsync(string username, Guid clinicId, CancellationToken token = default);
  public Task<Result<IReadOnlyList<GetUserResponse>>> GetAllUsersAsync(Guid clinicId, CancellationToken token = default);
  public Task<Result<GetUserResponse>> AddAsync(CreateUserRequest request, CancellationToken token = default);
  public Task<Result> UpdateAsync(int id, UpdateUserRequest request, CancellationToken token = default);
}
