using Npgsql;
using App.Domain.Entities; 
using App.Domain.ValueObjects;

namespace App.Infra.Persistence.Ado.Mappers;
public static class Mappers
{
  public static Clinic ToClinic(NpgsqlDataReader reader) 
  {
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
