namespace App.Application.DTOs;
public record class UpdateUserRequest
{
  public string Username { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string Role { get; set; } = null!;
  public bool IsActive { get; set; }
  public Guid ClinicId { get; set; }
}
