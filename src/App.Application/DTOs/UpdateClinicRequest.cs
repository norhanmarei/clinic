namespace App.Application.DTOs;
public class UpdateClinicRequest
{
  public string Name { get; set; } = null!;
  public string Timezone { get; set; } = null!;
  public TimeOnly Start { get; set; }
  public TimeOnly End { get; set; }
  public bool IsActive { get; set; }
}
