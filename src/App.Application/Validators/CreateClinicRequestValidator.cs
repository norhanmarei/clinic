using App.Application.DTOs;
using FluentValidation;
namespace App.Application.Validators;
public class CreateClinicRequestValidator : AbstractValidator<CreateClinicRequest>
{
  public CreateClinicRequestValidator()
  {
    RuleFor(x => x.Name)
      .Cascade(CascadeMode.Stop)
      .NotEmpty().WithMessage("Name is required")
      .MaximumLength(200);

    RuleFor(x => x.WorkingHours)
      .NotNull().WithMessage("Working hours are required");

    RuleFor(x => x.Timezone)
      .NotNull().WithMessage("Timezone is required");
  }
}
