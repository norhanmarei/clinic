using App.Application.DTOs;
using FluentValidation;

namespace App.Application.Validators;
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
  public CreateUserRequestValidator()
  {
    RuleFor(x => x.Username)
      .Cascade(CascadeMode.Stop)
      .NotEmpty().WithMessage("Username is required")
      .MaximumLength(100).WithMessage("Username must not exceed 100 characters");

    RuleFor(x => x.Password)
      .Cascade(CascadeMode.Stop)
      .NotEmpty().WithMessage("Password is required")
      .MinimumLength(15).WithMessage("Password must be at least 15 characters");

    RuleFor(x => x.Role)
      .Cascade(CascadeMode.Stop)
      .NotEmpty().WithMessage("Role is required")
      .Must(BeValid).WithMessage("Role must be either 'admin', 'doctor', or 'receptionist'");


    RuleFor(x => x.ClinicId)
      .NotEmpty().WithMessage("Clinic id is required");
  }
  private bool BeValid(string role) => role == "admin" || role == "doctor" || role == "receptionist";
}
