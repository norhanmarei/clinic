namespace App.Domain.ValueObjects
{
  public sealed class WorkingHours
  {
    public TimeOnly Start {get;}
    public TimeOnly End {get;}
    public WorkingHours(TimeOnly start, TimeOnly end)
    {
       if(end <= start) 
         throw new ArgumentException("Invalid working hours");

       Start = start;
       End = end;
    }

    public bool IsOpenAt(TimeOnly time) => time >= Start && time <= End;
  }
}
