using Npgsql;
using App.Domain.Entities; 
using App.Domain.ValueObjects;

namespace App.Infra.Persistence.Ado.Mappers;
public static class Mappers
{
  public static Clinic ToClinic(NpgsqlDataReader reader) 
  {
    int idOrd = 0;
    int nameOrd = 1;
    int tzOrd = 2;
    int startTimeOrd = 3;
    int endTimeOrd = 4;
    int isActiveOrd = 5;
    int createdAtOrd = 6;
    int updatedAtOrd = 7;

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
