namespace App.Application.Common.Results;
public sealed class Result<T>
{
  public bool IsSuccess { get; }
  public Error? Error { get; }
  public T? Value { get; }
  private Result(Error error)
  {
     IsSuccess = false;
     Error = error ?? throw new ArgumentNullException(nameof(error)); 
     Value = default;
  }
  private Result(T value)
  {
     IsSuccess = true;
     Value = value ?? throw new ArgumentNullException(nameof(value));
     Error = null;
  }
  public static Result<T> Success(T value) => new (value);
  public static Result<T> Failure(Error error) => new (error);
}
