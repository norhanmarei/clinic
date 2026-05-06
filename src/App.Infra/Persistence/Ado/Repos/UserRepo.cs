using App.Domain.Entities;
using App.Application.Interfaces.Repos;
using Microsoft.Extensions.Configuration;
using NpgsqlTypes;

namespace App.Infra.Persistence.Ado.Repos;
public class UserRepo(IConfiguration config): BaseRepo(config), IUserRepo
{
  public async Task<User?> GetByIdAsync(int id, CancellationToken token = default)
  {
    const string sql = "SELECT id, username, password_hash, role, is_active, clinic_id, created_at, updated_at FROM users WHERE id = @id";
    var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
    {
      ("@id", id, NpgsqlDbType.Integer),
    };
    return await ExecuteSingleReaderAsync<User?>(sql, parameters, Mappers.Mappers.ToUser, token);
  }

  public async Task<bool> Exists(string username, Guid clinicId, CancellationToken token = default)
  {
    const string sql = "SELECT EXISTS (SELECT 1 FROM users WHERE username = @username AND clinic_id = @clinic_id)";
    var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
    {
      ("@username", username, NpgsqlDbType.Varchar),
      ("@clinic_id", clinicId, NpgsqlDbType.Uuid),
    };
    return await ExecuteScalarAsync<bool>(sql, parameters, token);
  }

  public async Task<User?> GetByUsernameAsync(string username, Guid clinicId, CancellationToken token = default)
  {
    const string sql = @"SELECT 
      id, username, password_hash, role, is_active, clinic_id, created_at, updated_at FROM users 
      WHERE 
      username = @username AND clinic_id = @clinic_id";
    var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
    {
      ("@username", username, NpgsqlDbType.Varchar),
      ("@clinic_id", clinicId, NpgsqlDbType.Uuid),
    };
    return await ExecuteSingleReaderAsync<User?>(sql, parameters, Mappers.Mappers.ToUser, token);
  }

  public async Task<int> AddAsync(User user, CancellationToken token = default)
  {
    const string sql = @"INSERT INTO users 
      (username, password_hash, role, is_active, clinic_id, created_at, updated_at) 
      VALUES
      (@username, @password_hash, @role, @is_active, @clinic_id, @created_at, @updated_at)
      RETURNING id;";
    string role = user.Role switch 
    {
      Domain.Enums.Role.admin => "admin", 
      Domain.Enums.Role.doctor => "doctor", 
      Domain.Enums.Role.receptionist => "receptionist", 
      Domain.Enums.Role.unknown => "unknown", 
      _ => "unknown"
    };
    var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
    {
      ("@id", user.Id, NpgsqlDbType.Integer),
      ("@username", user.Username, NpgsqlDbType.Varchar),
      ("@password_hash", user.PasswordHash, NpgsqlDbType.Text),
      ("@role", role, NpgsqlDbType.Varchar),
      ("@is_active", user.IsActive, NpgsqlDbType.Boolean),
      ("@clinic_id", user.ClinicId, NpgsqlDbType.Uuid),
      ("@created_at", user.CreatedAt, NpgsqlDbType.TimestampTz),
      ("@updated_at", user.UpdatedAt, NpgsqlDbType.TimestampTz),
    };
    return await ExecuteScalarAsync<int>(sql, parameters, token);
  }

  public async Task<IReadOnlyList<User>> GetAllUsersAsync(Guid clinicId, CancellationToken token = default)
  {
    const string sql = @"SELECT 
      id, username, password_hash, role, is_active, clinic_id, created_at, updated_at FROM users 
      WHERE 
      clinic_id = @clinic_id";
    var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
    {
      ("@clinic_id", clinicId, NpgsqlDbType.Uuid),
    };
    return await ExecuteReaderAsync<User>(sql, parameters, Mappers.Mappers.ToUser, token);
  }
  public async Task<int> UpdateAsync(User user, CancellationToken token = default)
  {
    const string sql = @"Update users 
      SET
        username = @username, 
        password_hash = @password_hash,
        role = @role, 
        is_active = @is_active, 
        clinic_id = @clinic_id, 
        updated_at = @updated_at
      WHERE id = @id;";
    string role = user.Role switch 
    {
      Domain.Enums.Role.admin => "admin", 
      Domain.Enums.Role.doctor => "doctor", 
      Domain.Enums.Role.receptionist => "receptionist", 
      Domain.Enums.Role.unknown => "unknown", 
      _ => "unknown"
    };
    var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
    {
      ("@id", user.Id, NpgsqlDbType.Integer),
      ("@username", user.Username, NpgsqlDbType.Varchar),
      ("@password_hash", user.PasswordHash, NpgsqlDbType.Text),
      ("@role", role, NpgsqlDbType.Varchar),
      ("@is_active", user.IsActive, NpgsqlDbType.Boolean),
      ("@clinic_id", user.ClinicId, NpgsqlDbType.Uuid),
      ("@updated_at", user.UpdatedAt, NpgsqlDbType.TimestampTz),
    };
    return await ExecuteNonQueryAsync(sql, parameters, token);
  }
}
