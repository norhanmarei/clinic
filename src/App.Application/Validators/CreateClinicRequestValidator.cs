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

    RuleFor(x => x.StartWorkingHour)
      .NotNull().WithMessage("Start working hour is required");

    RuleFor(x => x.EndWorkingHour)
      .NotNull().WithMessage("End working hour is required");

    RuleFor(x => x.EndWorkingHour)
      .GreaterThan(wh => wh.StartWorkingHour)
      .WithMessage("End working hour must be greater than start working hour");

    RuleFor(x => x.Timezone)
      .NotNull().WithMessage("Timezone is required");
  }
}
