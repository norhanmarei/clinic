using App.Application.DTOs;
using FluentValidation;

namespace App.Application.Validators;
public class CreateDoctorRequestValidator: AbstractValidator<CreateDoctorRequest>
{
  public CreateDoctorRequestValidator()
  {
      RuleFor(x => x.FullName)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("Doctor full name is required")
        .MaximumLength(300).WithMessage("Full name max length is 300 characters");

      RuleFor(x => x.Specialty)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("Doctor Specialty is required")
        .MaximumLength(100).WithMessage("Specialty max length is 100 characters");

      RuleFor(x => x.Gender)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("Doctor gender is required")
        .MaximumLength(10).WithMessage("Gender max length is 10 characters")
        .Must(BeValid).WithMessage("Gender must either be 'female', 'male', or 'other'");

      RuleFor(x => x.UserId)
        .Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("Doctor user id is required")
        .GreaterThanOrEqualTo(0).WithMessage("User id must be greater than or equal 0");

      RuleFor(x => x.ClinicId)
        .NotEmpty().WithMessage("Doctor user id is required");
  }
  private bool BeValid(string gender)
    => gender == "male" || gender == "female" || gender == "other";
}
