namespace App.Application.Common.Results;
using App.Application.Enums;
public sealed class Error(ErrorType type, string message)
{
  public ErrorType Type { get; } = type;
  public string Message { get; } = message;
}
