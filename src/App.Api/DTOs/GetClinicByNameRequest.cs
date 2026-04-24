namespace App.Api.DTOs;
public record GetClinicByNameRequest
{
  public string Name { get; set; } = null!;
}
