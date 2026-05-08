namespace App.Domain.Entities;
public class Patient
{
  public int Id { get; private set; }

  public string FullName { get; private set; } = null!;
  public DateOnly BirthDate { get; private set; }

  public string Gender { get; private set; } = null!;
  public string Email { get; private set; } = null!;
  public string? PhoneNumber { get; private set; }

  public bool IsActive { get; private set; }

  public Guid ClinicId { get; private set; }

  public DateTimeOffset CreatedAt { get; private set; }
  public DateTimeOffset UpdatedAt { get; private set; }

  private Patient(){}
  private Patient(string fullName, DateOnly birthDate, string gender, string email, string? phoneNumber, bool isActive, Guid clinicId)
  {
    _SetName(fullName);
    ValidateBirthDate(birthDate);
    BirthDate = birthDate;
    Gender = gender.Trim() ?? throw new ArgumentNullException(nameof(gender));
    Email = email.Trim() ?? throw new ArgumentNullException(nameof(email));
    PhoneNumber = phoneNumber?.Trim();
    IsActive = isActive;
    ClinicId = clinicId != Guid.Empty ? clinicId : throw new ArgumentException("Domain: Clinic id is required");
    CreatedAt = Now();
    UpdatedAt = CreatedAt;
  }

  private Patient(int id, string fullName, DateOnly birthDate, string gender, string email, 
      string? phoneNumber, bool isActive, Guid clinicId, DateTimeOffset createdAt, DateTimeOffset updatedAt)
  {
    Id = id >= 0 ? id : throw new ArgumentException("Domain: patient id must be greater than or equal to 0");
    _SetName(fullName);
    ValidateBirthDate(birthDate);
    BirthDate = birthDate;
    Gender = gender.Trim() ?? throw new ArgumentNullException(nameof(gender));
    Email = email.Trim() ?? throw new ArgumentNullException(nameof(email));
    PhoneNumber = phoneNumber?.Trim();
    IsActive = isActive;
    ClinicId = clinicId != Guid.Empty ? clinicId : throw new ArgumentException("Domain: Clinic id is required");
    CreatedAt = createdAt;
    UpdatedAt = updatedAt;
  }

  public static Patient Create(string fullName, DateOnly birthDate, string gender, string email, string? phoneNumber, bool isActive, Guid clinicId)
    => new (fullName, birthDate, gender, email, phoneNumber, isActive, clinicId);

  public static Patient FromPersistence(int id, string fullName, DateOnly birthDate, string gender, string email, 
      string? phoneNumber, bool isActive, Guid clinicId, DateTimeOffset createdAt, DateTimeOffset updatedAt)
    => new (id, fullName, birthDate, gender, email, phoneNumber, isActive, clinicId, createdAt, updatedAt);

  private static DateTimeOffset Now() => DateTimeOffset.UtcNow;

  private void _SetName(string fullName)
  {
    if(string.IsNullOrWhiteSpace(fullName))
      throw new ArgumentException("Domain: Patient Name is required");
    FullName = fullName.Trim();
    UpdatedAt = Now();
  }

  private static void ValidateBirthDate(DateOnly birthDate)
  {
    if (birthDate > DateOnly.FromDateTime(DateTime.UtcNow))
      throw new ArgumentException("Domain: Birth date cannot be in the future");
  }

  public void ChangeName(string fullName)
  {
    if(string.IsNullOrWhiteSpace(fullName))
      throw new ArgumentException("Domain: Patient name is required");
    if(FullName == fullName.Trim()) return;
    FullName = fullName.Trim();
    UpdatedAt = Now();
  }

  public void ChangeGender(string gender)
  {
    if(string.IsNullOrWhiteSpace(gender))
      throw new ArgumentException("Domain: Patient gender is required");
    if(Gender == gender.Trim()) return;
    Gender = gender.Trim();
    UpdatedAt = Now();
  }

  public void ChangeEmail(string email)
  {
    if(string.IsNullOrWhiteSpace(email))
      throw new ArgumentException("Domain: Patient email is required");
    if(Email == email.Trim()) return;
    Email = email.Trim();
    UpdatedAt = Now();
  }

  public void ChangePhoneNumber(string? phoneNumber)
  {
    PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim();
    UpdatedAt = Now();
  }

  public void ChangeBirthDate(DateOnly birthDate)
  {
    if(BirthDate == birthDate) return;
    ValidateBirthDate(birthDate);
    BirthDate = birthDate;
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
