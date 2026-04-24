
namespace App.Application.DTOs;
public record GetClinicByNameRequest
{
  public string Name { get; set; } = null!;
}
