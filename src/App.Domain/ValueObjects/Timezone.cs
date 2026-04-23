namespace App.Domain.ValueObjects
{
  public sealed class Timezone
  {
    public string Id { get; }
    public TimeZoneInfo Info { get; }
    public Timezone(string id)
    {
       Info = TimeZoneInfo.FindSystemTimeZoneById(id); 
       Id = id;
    }
  }
}
