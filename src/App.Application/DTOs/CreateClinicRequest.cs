namespace App.Application.DTOs;
public class CreateClinicRequest
{
  public string Name { get; set; } = null!;
  public string Timezone { get; set; } = null!;
  public TimeOnly StartWorkingHour { get; set; }
  public TimeOnly EndWorkingHour { get; set; }
  public bool IsActive { get; set; }
}
