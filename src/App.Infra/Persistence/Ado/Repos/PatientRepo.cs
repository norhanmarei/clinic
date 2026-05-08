using App.Application.Interfaces.Repos;
using App.Domain.Entities;
using Microsoft.Extensions.Configuration;
using NpgsqlTypes;

namespace App.Infra.Persistence.Ado.Repos;
public class PatientRepo(IConfiguration config) : BaseRepo(config), IPatientRepo
{
  public async Task<Patient?> GetByIdAsync(int id, CancellationToken token = default)
  {
    const string sql = @"SELECT 
      id, full_name, birth_date, gender, email, phone_number, is_active, clinic_id, created_at, updated_at 
      FROM patients
      WHERE 
      id = @id;";
    var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
    {
      ("@id", id, NpgsqlDbType.Integer),
    };
    return await ExecuteSingleReaderAsync<Patient>(sql, parameters, Mappers.Mappers.ToPatient, token);
  }

  public async Task<bool> Exists(int id, CancellationToken token = default)
  {
    const string sql = @"SELECT EXISTS (SELECT 1 FROM patients WHERE id = @id)";

    var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
    {
      ("@id", id, NpgsqlDbType.Integer),
    };
    return await ExecuteScalarAsync<bool>(sql, parameters, token);
  }

  public async Task<IReadOnlyList<Patient>> GetPatientsPerClinicAsync(Guid clinicId, CancellationToken token = default)
  {
    const string sql = @"SELECT 
      id, full_name, birth_date, gender, email, phone_number, is_active, clinic_id, created_at, updated_at 
      FROM patients
      WHERE 
      clinic_id = @clinic_id;";
    var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
    {
      ("@clinic_id", clinicId, NpgsqlDbType.Uuid),
    };
    return await ExecuteReaderAsync<Patient>(sql, parameters, Mappers.Mappers.ToPatient, token);
  }

  // needs a join and a junction table
  // public async Task<IReadOnlyList<Patient>> GetPatientsPerDoctorAsync(int id, CancellationToken token = default)
  // {
  //   const string sql = @"SELECT 
  //     id, full_name, birth_date, gender, email, phone_number, is_active, clinic_id, created_at, updated_at 
  //     FROM patients
  //     WHERE 
  //      = @clinic_id;";
  //   var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
  //   {
  //     ("@clinic_id", clinicId, NpgsqlDbType.Uuid),
  //   };
  //   return await ExecuteReaderAsync<Patient>(sql, parameters, Mappers.Mappers.ToPatient, token);
  // }

  public async Task<int> AddAsync(Patient patient, CancellationToken token = default)
  {
    const string sql = @"INSERT INTO patients 
      (full_name, birth_date, gender, email, phone_number, is_active, clinic_id, created_at, updated_at)
      VALUES
      (@full_name, @birth_date, @gender, @email, @phone_number, @is_active, @clinic_id, @created_at, @updated_at)
      RETURNING id;";
    var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
    {
      ("@full_name", patient.FullName, NpgsqlDbType.Varchar),
      ("@birth_date", patient.BirthDate, NpgsqlDbType.Date),
      ("@gender", patient.Gender, NpgsqlDbType.Varchar),
      ("@email", patient.Email, NpgsqlDbType.Varchar),
      ("@phone_number", patient.PhoneNumber, NpgsqlDbType.Varchar),
      ("@is_active", patient.IsActive, NpgsqlDbType.Boolean),
      ("@clinic_id", patient.ClinicId, NpgsqlDbType.Uuid),
      ("@created_at", patient.CreatedAt, NpgsqlDbType.TimestampTz),
      ("@updated_at", patient.UpdatedAt, NpgsqlDbType.TimestampTz),
    };
    return await ExecuteScalarAsync<int>(sql, parameters, token);
  }

  public async Task<int> UpdateAsync(Patient patient, CancellationToken token = default)
  {
    const string sql = @"UPDATE patients 
      SET
      full_name = @full_name, birth_date = @birth_date, gender = @gender, email = @email, 
    phone_number = @phone_number, is_active = @is_active, clinic_id = @clinic_id
      WHERE id = @id;";
    var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
    {
      ("@id", patient.Id, NpgsqlDbType.Integer),
      ("@full_name", patient.FullName, NpgsqlDbType.Varchar),
      ("@birth_date", patient.BirthDate, NpgsqlDbType.Date),
      ("@gender", patient.Gender, NpgsqlDbType.Varchar),
      ("@email", patient.Email, NpgsqlDbType.Varchar),
      ("@phone_number", patient.PhoneNumber, NpgsqlDbType.Varchar),
      ("@is_active", patient.IsActive, NpgsqlDbType.Boolean),
      ("@clinic_id", patient.ClinicId, NpgsqlDbType.Uuid),
    };
    return await ExecuteNonQueryAsync(sql, parameters, token);
  }
}
