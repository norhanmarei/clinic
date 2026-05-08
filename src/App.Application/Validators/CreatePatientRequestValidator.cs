using App.Application.DTOs;
using FluentValidation;
namespace App.Application.Validators;
public class CreatePatientRequestValidator : AbstractValidator<CreatePatientRequest>
{
  public CreatePatientRequestValidator()
  {
    RuleFor(x => x.FullName)
      .Cascade(CascadeMode.Stop)
      .NotEmpty().WithMessage("patient full name is required")
      .MaximumLength(100).WithMessage("full name max length is 100 characters");

    RuleFor(x => x.Email)
      .Cascade(CascadeMode.Stop)
      .NotEmpty().WithMessage("patient email is required")
      .MaximumLength(100).WithMessage("email max length is 100 characters")
      .EmailAddress().WithMessage("invalid email address");

    RuleFor(x => x.Gender)
      .Cascade(CascadeMode.Stop)
      .NotEmpty().WithMessage("patient gender is required")
      .Must(BeValid).WithMessage("patient gender must either be 'female', 'male' or 'other'");

    RuleFor(x => x.BirthDate)
      .Cascade(CascadeMode.Stop)
      .NotEmpty().WithMessage("patient birth date is required")
      .Must(date => date <= DateOnly.FromDateTime(DateTime.UtcNow))
      .WithMessage("patient birth date must not be in the future");

    RuleFor(x => x.PhoneNumber)
      .MaximumLength(20).WithMessage("phone number max length is 20 characters")
      .Matches(@"^\+?[0-9\s\-]*$").When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
      .WithMessage("invalid phone number format");

    RuleFor(x => x.ClinicId)
      .NotEmpty().WithMessage("patient clinic id is required");
  }
  private bool BeValid(string gender) 
    => gender.ToLower() is "female" or "male" or "other";
}
