namespace App.Domain.Entities;
public class Doctor
{
  public int Id { get; private set; }

  public string FullName { get; private set; } = null!;
  public string Specialty { get; private set; } = null!;
  public string Gender { get; private set; } = null!;

  public bool IsActive { get; private set; }

  public int UserId { get; private set; }
  public Guid ClinicId { get; private set; }

  public DateTimeOffset CreatedAt { get; private set; }
  public DateTimeOffset UpdatedAt { get; private set; }

  private Doctor(){}
  private Doctor(string fullName, string specialty, string gender, bool isActive, int userId, Guid clinicId)
  {
    _SetName(fullName);
    Specialty = specialty ?? throw new ArgumentNullException(nameof(specialty));
    Gender = gender ?? throw new ArgumentNullException(nameof(gender));
    IsActive = isActive;
    UserId = userId >= 0 ? userId : throw new ArgumentException("Domain: user id must be greater than or equal to 0");
    ClinicId = clinicId != Guid.Empty ? clinicId : throw new ArgumentException("Domain: Clinic id is required");
    CreatedAt = Now();
    UpdatedAt = CreatedAt;
  }

  private Doctor(int id, string fullName, string specialty, string gender, bool isActive, int userId, Guid clinicId, DateTimeOffset createdAt, DateTimeOffset updatedAt)
  {
    Id = id >= 0 ? id : throw new ArgumentException("Domain: doctor id must be greater than or equal to 0");
    _SetName(fullName);
    Specialty = specialty ?? throw new ArgumentNullException(nameof(specialty));
    Gender = gender ?? throw new ArgumentNullException(nameof(gender));
    IsActive = isActive;
    UserId = userId >= 0 ? userId : throw new ArgumentException("Domain: User id must be greater than or equal to 0");
    ClinicId = clinicId != Guid.Empty ? clinicId : throw new ArgumentException("Domain: Clinic id is required");
    CreatedAt = createdAt;
    UpdatedAt = updatedAt;
  }

  public static Doctor Create(string fullName, string specialty, string gender, bool isActive, int userId, Guid clinicId)
    => new (fullName, specialty, gender, isActive, userId, clinicId);

  public static Doctor FromPersistence(int id, string fullName, string specialty, string gender, bool isActive, int userId, Guid clinicId, DateTimeOffset createdAt, DateTimeOffset updatedAt)
    => new (id, fullName, specialty, gender, isActive, userId, clinicId, createdAt, updatedAt);

  private static DateTimeOffset Now() => DateTimeOffset.UtcNow;

  private void _SetName(string fullName)
  {
    if(string.IsNullOrWhiteSpace(fullName))
      throw new ArgumentException("Domain: Doctor Name is required");
    FullName = fullName;
    UpdatedAt = Now();
  }

  public void ChangeName(string fullName)
  {
    if(string.IsNullOrWhiteSpace(fullName))
      throw new ArgumentException("Domain: Doctor name is required");
    if(FullName == fullName.Trim()) return;
    FullName = fullName.Trim();
    UpdatedAt = Now();
  }

  public void ChangeSpecialty(string specialty)
  {
    if(string.IsNullOrWhiteSpace(specialty))
      throw new ArgumentException("Domain: Doctor specialty is required");
    if(Specialty == specialty.Trim()) return;
    Specialty = specialty.Trim();
    UpdatedAt = Now();
  }

  public void ChangeGender(string gender)
  {
    if(string.IsNullOrWhiteSpace(gender))
      throw new ArgumentException("Domain: Doctor gender is required");
    if(Gender == gender.Trim()) return;
    Gender = gender.Trim();
    UpdatedAt = Now();
  }

  public void ChangeUserId(int id)
  {
    if(id < 0)
      throw new ArgumentException("Domain: user id must be greater than or equal to 0");
    if(UserId == id) return;
    UserId = id;
    UpdatedAt = Now();
  }

  public void ChangeClinicId(Guid clinicId)
  {
    if(clinicId == Guid.Empty)
      throw new ArgumentException("Domain: clinic id is required");
    if(clinicId == ClinicId) return;
    ClinicId = clinicId;
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
