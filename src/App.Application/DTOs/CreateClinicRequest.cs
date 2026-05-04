using App.Domain.ValueObjects;

namespace App.Application.DTOs;
public class CreateClinicRequest
{
  public string Name { get; set; } = null!;
  public Timezone Timezone { get; set; } = null!;
  public WorkingHours WorkingHours { get; set; } = null!;
  public bool IsActive { get; set; }
}
