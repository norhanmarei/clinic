using App.Application.Common.Results;
using App.Application.DTOs;
using App.Application.Interfaces.Repos;
using App.Application.Interfaces.Security;
using App.Application.Interfaces.Services;
using App.Domain.Entities;
using App.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace App.Application.Services;
public class UserService(ILogger<UserService> logger, IUserRepo repo, IPasswordHasher hasher): IUserService
{
  private readonly ILogger _logger = logger;
  private readonly IUserRepo _repo = repo;
  private readonly IPasswordHasher _hasher = hasher;
  public async Task<Result<GetUserResponse>> GetByIdAsync(int id, CancellationToken token = default)
  {
    if(id < 0) 
    {
      _logger.LogWarning("User id must be greater than or equal to 0");
      return Result<GetUserResponse>.Failure(new Error(Enums.ErrorType.BadRequest, "User id must be greater than or equal to 0"));
    }

    var user = await _repo.GetByIdAsync(id, token);
    if(user is null)
    {
      _logger.LogWarning("User with id [{Id}] was not found", id);
      return Result<GetUserResponse>.Failure(new Error(Enums.ErrorType.NotFound, $"User with id [{id}] was not found"));
    }

    return Result<GetUserResponse>.Success(Mapping.Mappers.ToUserResponse(user));
  }

  public async Task<Result<GetUserResponse>> GetByUsernameAsync(string username, Guid clinicId, CancellationToken token = default)
  {
    if(string.IsNullOrWhiteSpace(username)) 
    {
      _logger.LogWarning("Username is required");
      return Result<GetUserResponse>.Failure(new Error(Enums.ErrorType.BadRequest, "Username is required"));
    }

    if(username.Length > 100) 
      return Result<GetUserResponse>.Failure(new Error(Enums.ErrorType.BadRequest, "Username must not exceed 100 characters"));

    if(clinicId == Guid.Empty)
    {
      _logger.LogWarning("Clinic id is required");
      return Result<GetUserResponse>.Failure(new Error(Enums.ErrorType.BadRequest, "Clinic id is required"));
    }

    var user = await _repo.GetByUsernameAsync(username, clinicId, token);
    if(user is null)
    {
      _logger.LogWarning("User with [{Username}] was not found", username);
      return Result<GetUserResponse>.Failure(new Error(Enums.ErrorType.NotFound, $"User with username [{username}] was not found"));
    }

    return Result<GetUserResponse>.Success(Mapping.Mappers.ToUserResponse(user));
  }

  public async Task<Result<IReadOnlyList<GetUserResponse>>> GetAllUsersAsync(Guid clinicId, CancellationToken token = default)
  {
    if(clinicId == Guid.Empty) 
      return Result<IReadOnlyList<GetUserResponse>>.Failure(new Error(Enums.ErrorType.BadRequest, "Clinic id is required"));

    var users = await _repo.GetAllUsersAsync(clinicId, token);
    return Result<IReadOnlyList<GetUserResponse>>.Success(Mapping.Mappers.ToUserResponse(users));
  }

  public async Task<Result<GetUserResponse>> AddAsync(CreateUserRequest request, CancellationToken token = default)
  {
    if(await _repo.Exists(request.Username, request.ClinicId, token))
    {
      _logger.LogWarning("Conflict: username [{Username}] already exists", request.Username.Trim());
      return Result<GetUserResponse>.Failure(new Error(Enums.ErrorType.Conflict, $"Conflict: username [{request.Username}] already exists"));
    }
    string passowrdHash = _hasher.Hash(request.Password.Trim());
    Role roleEnum = request.Role.Trim() switch 
    {
      "admin" => Role.admin, 
      "doctor" => Role.doctor, 
      "receptionist" => Role.receptionist, 
      "unknown" => Role.unknown, 
      _ => Role.unknown
    };
    var user = User.Create(request.Username.Trim(), passowrdHash, roleEnum, request.IsActive, request.ClinicId);
    int id = await _repo.AddAsync(user, token);
    var dto = new GetUserResponse
    {
      Id = id,
      Username = user.Username,
      Role = request.Role.Trim(),
      IsActive = user.IsActive,
      ClinicId = user.ClinicId,
      CreatedAt = user.CreatedAt,
      UpdatedAt = user.UpdatedAt
    };
    return Result<GetUserResponse>.Success(dto);
  }

  public async Task<Result> UpdateAsync(int id, UpdateUserRequest request, CancellationToken token = default)
  {
    if(id < 0) 
    {
      _logger.LogInformation("User id [{Id}] must be greater than or equal to 0", id);
      return Result.Failure(new Error(Enums.ErrorType.BadRequest, "Bad Request: User id must be greater than or equal to 0"));
    }
    var user = await _repo.GetByIdAsync(id, token);
    if(user is null)
    {
      _logger.LogWarning("User with id [{Id}] was not found", id);
      return Result.Failure(new Error(Enums.ErrorType.NotFound, $"User with id [{id}] was not found"));
    }
    user.ChangeUsername(request.Username);
    string passwordHash = _hasher.Hash(request.Password);
    user.ChangePassword(passwordHash);
    Role roleEnum = request.Role.Trim() switch 
    {
      "admin" => Role.admin, 
      "doctor" => Role.doctor, 
      "receptionist" => Role.receptionist, 
      "unknown" => Role.unknown, 
      _ => Role.unknown
    };
    user.ChangeRole(roleEnum);
    user.ChangeClinicId(request.ClinicId);

    if(request.IsActive && !user.IsActive) user.Activate();
    if(!request.IsActive && user.IsActive) user.Deactivate();

    int affectedRows = await _repo.UpdateAsync(user, token);
    if(affectedRows < 1)
    {
      _logger.LogWarning("Unexpected: failed to update user with [{Id}]", id);
      return Result.Failure(new Error(Enums.ErrorType.Unexpected, $"Unexpected: failed to update user with [{id}]"));
    }
    return Result.Success();
  }
}
