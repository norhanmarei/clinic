using App.Domain.Enums;

namespace App.Application.DTOs;
public record class GetUserResponse
{
  public int Id { get; set; }
  public string Username { get; set; } = null!;
  public string Role { get; set; } = null!;
  
  public bool IsActive { get; set; }
  public Guid ClinicId { get; set; }

  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset UpdatedAt { get; set; }
}
