using App.Domain.ValueObjects;
namespace App.Domain.Entities
{
  public class Clinic
  {
    public Guid Id { get; private set; }

    public string Name {get; private set;} = null!;

    public Timezone Timezone { get; private set; } = null!;
    public bool IsActive { get; private set; }

    public WorkingHours WorkingHours { get; private set; } = null!;

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    private Clinic() { } 

    private Clinic(
        string name,
        Timezone timezone,
        WorkingHours workingHours)
    {
      Id = Guid.NewGuid();
      _SetName(name);

      Timezone = timezone ?? throw new ArgumentNullException(nameof(timezone));
      WorkingHours = workingHours ?? throw new ArgumentNullException(nameof(workingHours));
      IsActive = true;

      CreatedAt = Now();
      UpdatedAt = CreatedAt;
    }

    private void _SetName(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        throw new ArgumentException("Clinic name is required");

      Name = name;
    }

    private static DateTimeOffset Now() => DateTimeOffset.UtcNow;

    public static Clinic Create(string name, Timezone timezone, WorkingHours workingHours) 
      => new Clinic(name, timezone, workingHours);

    public static Clinic FromPersistence
      (Guid id, string name, Timezone timezone, 
       WorkingHours workingHours, bool isActive, 
       DateTimeOffset createdAt, DateTimeOffset updatedAt)
      {
        var clinic = new Clinic();
        clinic.Id = id;
        clinic._SetName(name);
        clinic.Timezone = timezone ?? throw new ArgumentNullException(nameof(timezone));
        clinic.WorkingHours = workingHours ?? throw new ArgumentNullException(nameof(workingHours));
        clinic.IsActive = isActive;
        clinic.CreatedAt = createdAt;
        clinic.UpdatedAt = updatedAt;
        return clinic;
      }
    public void ChangeName(string name)
    {
      _SetName(name);
      UpdatedAt = Now();
    }

    public void Activate()
    {
      if(IsActive) return;
      IsActive = true;
      UpdatedAt = Now();
    }

    public void Deactivate()
    {
      if(!IsActive) return;
      IsActive = false;
      UpdatedAt = Now();
    }
  }
}
