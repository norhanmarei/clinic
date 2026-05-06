using App.Application.DTOs;
using App.Domain.Entities;
namespace App.Application.Mapping;
public static class Mappers
{
  public static GetClinicResponse ToClinicResponse(Clinic clinic)
  {
    return new GetClinicResponse
        {
        Id = clinic.Id, 
        Name = clinic.Name,
        Timezone = clinic.Timezone,
        IsActive = clinic.IsActive,
        WorkingHours = clinic.WorkingHours,
        CreatedAt = clinic.CreatedAt,
        UpdatedAt = clinic.UpdatedAt
        };
  }
  public static IEnumerable<GetClinicResponse> ToClinicResponse(IEnumerable<Clinic> list)
  {
    var result = new List<GetClinicResponse>();
    foreach(var clinic in list)
    {
      result.Add(ToClinicResponse(clinic));
    }
    return result;
  }

  public static GetUserResponse ToUserResponse(User user)
  {
    string role = user.Role switch 
    {
      Domain.Enums.Role.admin => "admin", 
      Domain.Enums.Role.doctor => "doctor", 
      Domain.Enums.Role.receptionist => "receptionist", 
      Domain.Enums.Role.unknown => "unknown", 
      _ => "unknown"
    };
    return new GetUserResponse
        {
        Id = user.Id, 
        Username = user.Username,
        Role = role,
        ClinicId = user.ClinicId,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt
        };
  }

  public static IReadOnlyList<GetUserResponse> ToUserResponse(IReadOnlyList<User> list)
  {
    var result = new List<GetUserResponse>();
    foreach(var user in list)
    {
      result.Add(ToUserResponse(user));
    }
    return result;
  }

  public static GetDoctorResponse ToDoctorResponse(Doctor doctor)
  {
    return new GetDoctorResponse
        {
        Id = doctor.Id, 
        FullName = doctor.FullName,
        Specialty = doctor.Specialty,
        Gender = doctor.Gender,
        IsActive = doctor.IsActive,
        UserId = doctor.UserId,
        ClinicId = doctor.ClinicId,
        CreatedAt = doctor.CreatedAt,
        UpdatedAt = doctor.UpdatedAt
        };
  }

  public static IReadOnlyList<GetDoctorResponse> ToDoctorResponse(IReadOnlyList<Doctor> list)
  {
    var result = new List<GetDoctorResponse>();
    foreach(var doctor in list)
    {
      result.Add(ToDoctorResponse(doctor));
    }
    return result;
  }
}
