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
}
