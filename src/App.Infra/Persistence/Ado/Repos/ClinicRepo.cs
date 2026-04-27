using Microsoft.Extensions.Configuration;
using App.Domain.Entities;
using App.Domain.ValueObjects;
using App.Application.Interfaces.Repos;
using Npgsql;
namespace App.Infra.Persistence.Ado.Repos
{
  public class ClinicRepo(IConfiguration config) : IClinicRepo
  {
    private readonly string _connectionString = 
        config.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Missing connection string 'DefaultConnection'");

    public async Task<Clinic?> GetByNameAsync(string name, CancellationToken token = default)
    {
      await using NpgsqlConnection conn = new (_connectionString);
      const string sql = "SELECT id, name, timezone, start_time, end_time, is_active, created_at, updated_at FROM clinics WHERE name=@name";
      await using NpgsqlCommand cmd = new (sql, conn);
      cmd.Parameters.AddWithValue("@name", name);
      await conn.OpenAsync(token);
      await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync(token);
      if(!await reader.ReadAsync(token)) return null;

      int idOrd = reader.GetOrdinal("id");
      int nameOrd = reader.GetOrdinal("name");
      int tzOrd = reader.GetOrdinal("timezone");
      int startTimeOrd = reader.GetOrdinal("start_time");
      int endTimeOrd = reader.GetOrdinal("end_time");
      int isActiveOrd = reader.GetOrdinal("is_active");
      int createdAtOrd = reader.GetOrdinal("created_at");
      int updatedAtOrd = reader.GetOrdinal("updated_at");

      Guid id = reader.GetGuid(idOrd);
      string clinicName = reader.GetString(nameOrd);
      Timezone timezone = new (reader.GetString(tzOrd));

      TimeOnly start = TimeOnly.FromTimeSpan(reader.GetTimeSpan(startTimeOrd));
      TimeOnly end = TimeOnly.FromTimeSpan(reader.GetTimeSpan(endTimeOrd));
      WorkingHours workingHours = new (start, end);

      bool isActive = reader.GetBoolean(isActiveOrd);

      DateTimeOffset createdAt = reader.GetFieldValue<DateTimeOffset>(createdAtOrd);
      DateTimeOffset updatedAt = reader.GetFieldValue<DateTimeOffset>(updatedAtOrd);

      return Clinic.FromPersistence(id, clinicName, timezone, workingHours, isActive, createdAt, updatedAt);
    }
  }
}
