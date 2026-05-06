namespace App.Application.DTOs;
public record class GetDoctorResponse
{
  public int Id { get; set; }

  public string FullName { get; set; } = null!;
  public string Specialty { get; set; } = null!;
  public string Gender { get; set; } = null!;

  public bool IsActive { get; set; }

  public int UserId { get; set; }
  public Guid ClinicId { get; set; }

  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset UpdatedAt { get; set; }
}
