namespace App.Application.DTOs;
public record class CreateDoctorRequest
{
  public string FullName { get;  set; } = null!;
  public string Specialty { get;  set; } = null!;
  public string Gender { get;  set; } = null!;

  public bool IsActive { get;  set; }

  public int UserId { get;  set; }
  public Guid ClinicId { get;  set; }
}
