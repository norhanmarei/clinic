using App.Domain.ValueObjects;
namespace App.Application.DTOs;
public record GetClinicByNameResponse
{
    public Guid Id { get; init; }

    public string Name {get; init;} = null!;

    public Timezone Timezone { get; init; } = null!;
    public bool IsActive { get; init; }

    public WorkingHours WorkingHours { get; init; } = null!;

    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }

}
