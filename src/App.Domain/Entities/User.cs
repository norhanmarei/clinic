using App.Domain.Enums;

namespace App.Domain.Entities;
public class User
{
  public int Id { get; private set; }

  public string Username { get; private set; } = null!;
  public string PasswordHash { get; private set; } = null!;

  public Role Role { get; private set; }

  public bool IsActive { get; private set; }
  public Guid ClinicId { get; private set; }

  public DateTimeOffset CreatedAt { get; private set; }
  public DateTimeOffset UpdatedAt { get; private set; }

  private User(){}
  private User(string username, string passwordHash, Role role, bool isActive, Guid clinicId)
  {
    _SetUsername(username);
    PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
    Role = role;
    IsActive = isActive;
    ClinicId = clinicId;

    CreatedAt = Now();
    UpdatedAt = CreatedAt;
  }
  private User(int id, string username, string passwordHash, Role role, bool isActive, Guid clinicId, DateTimeOffset createdAt, DateTimeOffset updatedAt)
  {
    Id = id;
    _SetUsername(username);
    PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
    Role = role;
    IsActive = isActive;
    ClinicId = clinicId;
    CreatedAt = createdAt;
    UpdatedAt = updatedAt;
  }

  public static User Create(string username, string passwordHash, Role role, bool isActive, Guid clinicId) 
    => new (username, passwordHash, role, isActive, clinicId);

  public static User FromPersistence(int id, string username, string passwordHash, Role role, bool isActive, Guid clinicid, DateTimeOffset createdAt, DateTimeOffset updatedAt) 
    => new (id, username, passwordHash, role, isActive, clinicid, createdAt, updatedAt);

  private void _SetUsername(string username)
  {
    if (string.IsNullOrWhiteSpace(username))
      throw new ArgumentException("username is required");

    Username = username;
  }

  private static DateTimeOffset Now() => DateTimeOffset.UtcNow;

  public void ChangeUsername(string username)
  {
    if(Username == username) return;
    _SetUsername(username);
    UpdatedAt = Now();
  }

  public void ChangePassword(string passwordHash)
  {
    if(PasswordHash == passwordHash) return;
    if (string.IsNullOrWhiteSpace(passwordHash))
      throw new ArgumentException("password is required");
    PasswordHash = passwordHash;
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
