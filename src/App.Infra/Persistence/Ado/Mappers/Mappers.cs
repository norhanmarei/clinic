using Npgsql;
using App.Domain.Entities; 
using App.Domain.ValueObjects;
using App.Domain.Enums;

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

  public static User ToUser(NpgsqlDataReader reader) 
  {
    int idOrd = 0;
    int usernameOrd = 1;
    int passwordHashOrd = 2;
    int roleOrd = 3;
    int isActiveOrd = 4;
    int clinicIdOrd = 5;
    int createdAtOrd = 6;
    int updatedAtOrd = 7;

    int id = reader.GetInt32(idOrd);

    string username = reader.GetString(usernameOrd);
    string passwordHash = reader.GetString(passwordHashOrd);
    string role = reader.GetString(roleOrd);
    

    bool isActive = reader.GetBoolean(isActiveOrd);

    Guid clinicId = reader.GetGuid(clinicIdOrd);

    DateTimeOffset createdAt = reader.GetFieldValue<DateTimeOffset>(createdAtOrd);
    DateTimeOffset updatedAt = reader.GetFieldValue<DateTimeOffset>(updatedAtOrd);

    Role roleEnum = role switch
    {
      "admin" => Role.admin, 
      "doctor" => Role.doctor, 
      "receptionist" => Role.receptionist, 
      _ => Role.unknown
    };

    return User.FromPersistence(id, username, passwordHash, roleEnum, isActive, clinicId, createdAt, updatedAt);
  }
}
