using Microsoft.Extensions.Configuration;
using App.Domain.Entities;
using App.Application.Interfaces.Repos;
using NpgsqlTypes;
namespace App.Infra.Persistence.Ado.Repos
{
  public class ClinicRepo(IConfiguration config) : BaseRepo(config), IClinicRepo
  {

    public async Task<Clinic?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
      const string sql = "SELECT id, name, timezone, start_time, end_time, is_active, created_at, updated_at FROM clinics WHERE id=@id";
      var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
      {
        ("@id", id, NpgsqlDbType.Uuid),
      };  
      return await ExecuteSingleReaderAsync<Clinic?>(sql, parameters, Mappers.Mappers.ToClinic, token);
    }

    public async Task<Clinic?> GetByNameAsync(string name, CancellationToken token = default)
    {
      const string sql = "SELECT id, name, timezone, start_time, end_time, is_active, created_at, updated_at FROM clinics WHERE name=@name";
      var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
      {
        ("@name", name, NpgsqlDbType.Varchar),
      };  
      return await ExecuteSingleReaderAsync<Clinic?>(sql, parameters, Mappers.Mappers.ToClinic, token);
    }

    public async Task<IReadOnlyList<Clinic>> GetAllAsync(int offset, int limit, CancellationToken token = default)
    {
      const string sql = "SELECT id, name, timezone, start_time, end_time, is_active, created_at, updated_at FROM clinics ORDER By id LIMIT @limit OFFSET @offset";
      var parameters = new (string Name, object? Value, NpgsqlDbType Type)[] 
      {
        ("@limit", limit, NpgsqlDbType.Integer), 
        ("@offset", offset, NpgsqlDbType.Integer)
      };
      return await ExecuteReaderAsync<Clinic>(sql, parameters, Mappers.Mappers.ToClinic, token);

    }

    ///<returns> The number of affected rows in database. 
    public async Task<int> AddAsync(Clinic clinic, CancellationToken token = default)
    {
      const string sql = @"INSERT INTO clinics 
                           (id, name, timezone, start_time, end_time, is_active, created_at, updated_at)
                           VALUES
                           (@id, @name, @timezone, @start_time, @end_time, @is_active, @created_at, @updated_at)
                          ";
      var parameters = new (string Name, object? Value, NpgsqlDbType Type)[]
      {
        ("@id", clinic.Id, NpgsqlDbType.Uuid),
        ("@name", clinic.Name, NpgsqlDbType.Varchar),
        ("@timezone", clinic.Timezone.Id, NpgsqlDbType.Varchar),
        ("@start_time", clinic.WorkingHours.Start, NpgsqlDbType.Time),
        ("@end_time", clinic.WorkingHours.End, NpgsqlDbType.Time),
        ("@is_active", clinic.IsActive, NpgsqlDbType.Boolean),
        ("@created_at", clinic.CreatedAt, NpgsqlDbType.TimestampTz),
        ("@updated_at", clinic.UpdatedAt, NpgsqlDbType.TimestampTz),
      };
      return await ExecuteNonQueryAsync(sql, parameters, token); 
    }

    public async Task<bool> Exists(string name, CancellationToken token = default)
    {
      const string sql = @"SELECT EXISTS (SELECT 1 FROM clinics WHERE name = @name)";
      var parameters = new (string Name, object? Value, NpgsqlDbType Type)[]
      {
        ("@name", name, NpgsqlDbType.Varchar),
      };
      var res = await ExecuteScalarAsync<bool>(sql, parameters, token); 
      return res;
    }

    public async Task<bool> Exists(Guid id, CancellationToken token = default)
    {
      const string sql = @"SELECT EXISTS (SELECT 1 FROM clinics WHERE id = @id)";
      var parameters = new (string Name, object? Value, NpgsqlDbType Type)[]
      {
        ("@id", id, NpgsqlDbType.Uuid),
      };
      var res = await ExecuteScalarAsync<bool>(sql, parameters, token); 
      return res;
    }

    ///<returns> The number of affected rows in database. 
    public async Task<int> UpdateAsync(Clinic clinic, CancellationToken token = default)
    {
      const string sql = @"UPDATE clinics 
                           SET
                               name = @name, 
                               timezone = @timezone,
                               is_active = @is_active,
                               start_time = @start_time,
                               end_time = @end_time,
                               updated_at = @updated_at
                           WHERE id = @id";

      var parameters = new (string Name, object? Value, NpgsqlDbType Type)[]
      {
        ("@id", clinic.Id, NpgsqlDbType.Uuid),
        ("@name", clinic.Name, NpgsqlDbType.Varchar),
        ("@timezone", clinic.Timezone.Id, NpgsqlDbType.Varchar),
        ("@start_time", clinic.WorkingHours.Start, NpgsqlDbType.Time),
        ("@end_time", clinic.WorkingHours.End, NpgsqlDbType.Time),
        ("@is_active", clinic.IsActive, NpgsqlDbType.Boolean),
        ("@updated_at", clinic.UpdatedAt, NpgsqlDbType.TimestampTz),
      };

      var affectedRows = await ExecuteNonQueryAsync(sql, parameters, token);
      return affectedRows;
    }
  }
}
