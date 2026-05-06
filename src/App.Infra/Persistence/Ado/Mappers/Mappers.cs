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

  public static Doctor ToDoctor(NpgsqlDataReader reader) 
  {
    int idOrd = reader.GetOrdinal("id");
    int userIdOrd = reader.GetOrdinal("user_id");

    int clinicIdOrd = reader.GetOrdinal("clinic_id");
    int fullNameOrd = reader.GetOrdinal("full_name");

    int specialtyOrd = reader.GetOrdinal("specialty");
    int genderOrd = reader.GetOrdinal("gender");
    int isActiveOrd = reader.GetOrdinal("is_active");

    int createdAtOrd = reader.GetOrdinal("created_at");
    int updatedAtOrd = reader.GetOrdinal("updated_at");

    int id = reader.GetInt32(idOrd);
    int userId = reader.GetInt32(userIdOrd);
    Guid clinicId = reader.GetGuid(clinicIdOrd);

    string fullname = reader.GetString(fullNameOrd);
    string specialty = reader.GetString(specialtyOrd);
    string gender = reader.GetString(genderOrd);

    bool isActive = reader.GetBoolean(isActiveOrd);

    DateTimeOffset createdAt = reader.GetFieldValue<DateTimeOffset>(createdAtOrd);
    DateTimeOffset updatedAt = reader.GetFieldValue<DateTimeOffset>(updatedAtOrd);

    return Doctor.FromPersistence(id, fullname, specialty, gender, isActive, userId, clinicId, createdAt, updatedAt);
  }
}
