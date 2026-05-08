namespace App.Application.DTOs;
public record class CreatePatientRequest
{
  public string FullName { get; set; } = null!;
  public DateOnly BirthDate { get; set; }

  public string Gender { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string? PhoneNumber { get; set; }

  public bool IsActive { get; set; }

  public Guid ClinicId { get; set; }
}
