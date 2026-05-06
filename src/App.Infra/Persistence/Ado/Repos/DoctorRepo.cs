using App.Application.Interfaces.Repos;
using App.Domain.Entities;
using Microsoft.Extensions.Configuration;
using NpgsqlTypes;

namespace App.Infra.Persistence.Ado.Repos;
public class DoctorRepo(IConfiguration config) : BaseRepo(config), IDoctorRepo
{
  public async Task<Doctor?> GetByIdAsync(int id, CancellationToken token = default)
  {
    const string sql = @"SELECT 
                          id, full_name, specialty, gender, is_active, user_id, clinic_id, created_at, updated_at 
                          FROM doctors
                          WHERE 
                          id = @id;";
    var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
    {
      ("@id", id, NpgsqlDbType.Integer),
    };
    return await ExecuteSingleReaderAsync<Doctor>(sql, parameters, Mappers.Mappers.ToDoctor, token);
  }

  public async Task<bool> Exists(int id, CancellationToken token = default)
  {
    const string sql = @"SELECT EXISTS (SELECT 1 FROM doctors WHERE id = @id)";

    var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
    {
      ("@id", id, NpgsqlDbType.Integer),
    };
    return await ExecuteScalarAsync<bool>(sql, parameters, token);
  }

  public async Task<IReadOnlyList<Doctor>> GetAllDoctorsAsync(Guid clinicId, CancellationToken token = default)
  {
    const string sql = @"SELECT 
                          id, full_name, specialty, gender, is_active, user_id, clinic_id, created_at, updated_at 
                          FROM doctors
                          WHERE 
                          clinic_id = @clinic_id;";
    var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
    {
      ("@clinic_id", clinicId, NpgsqlDbType.Uuid),
    };
    return await ExecuteReaderAsync<Doctor>(sql, parameters, Mappers.Mappers.ToDoctor, token);
  }

  public async Task<int> AddAsync(Doctor doctor, CancellationToken token = default)
  {
    const string sql = @"INSERT INTO doctors 
                          (full_name, specialty, gender, is_active, user_id, clinic_id, created_at, updated_at)
                          VALUES
                          (@full_name, @specialty, @gender, @is_active, @user_id, @clinic_id, @created_at, @updated_at)
                          RETURNING id;";
    var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
    {
      ("@full_name", doctor.FullName, NpgsqlDbType.Varchar),
      ("@specialty", doctor.Specialty, NpgsqlDbType.Varchar),
      ("@gender", doctor.Gender, NpgsqlDbType.Varchar),
      ("@is_active", doctor.IsActive, NpgsqlDbType.Boolean),
      ("@user_id", doctor.UserId, NpgsqlDbType.Integer),
      ("@clinic_id", doctor.ClinicId, NpgsqlDbType.Uuid),
      ("@created_at", doctor.CreatedAt, NpgsqlDbType.TimestampTz),
      ("@updated_at", doctor.UpdatedAt, NpgsqlDbType.TimestampTz),
    };
    return await ExecuteScalarAsync<int>(sql, parameters, token);
  }

  public async Task<int> UpdateAsync(Doctor doctor, CancellationToken token = default)
  {
    const string sql = @"UPDATE doctors 
                         SET
                           full_name = @full_name, specialty = @specialty, gender = @gender, is_active = @is_active, user_id = @user_id, clinic_id = @clinic_id
                          WHERE id = @id;";
    var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
    {
      ("@id", doctor.Id, NpgsqlDbType.Integer),
      ("@full_name", doctor.FullName, NpgsqlDbType.Varchar),
      ("@specialty", doctor.Specialty, NpgsqlDbType.Varchar),
      ("@gender", doctor.Gender, NpgsqlDbType.Varchar),
      ("@is_active", doctor.IsActive, NpgsqlDbType.Boolean),
      ("@user_id", doctor.UserId, NpgsqlDbType.Integer),
      ("@clinic_id", doctor.ClinicId, NpgsqlDbType.Uuid),
    };
    return await ExecuteNonQueryAsync(sql, parameters, token);
  }
}
