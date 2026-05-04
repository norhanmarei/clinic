using App.Application.DTOs;
using FluentValidation;
namespace App.Application.Validators;
public class UpdateClinicRequestValidator : AbstractValidator<UpdateClinicRequest>
{
  public UpdateClinicRequestValidator()
  {
    RuleFor(x => x.Name)
      .Cascade(CascadeMode.Stop)
      .NotEmpty().WithMessage("Name is required")
      .MaximumLength(200);

    RuleFor(x => x.Start)
      .NotNull().WithMessage("Start working hour is required");

    RuleFor(x => x.End)
      .NotNull().WithMessage("End working hour is required");

    RuleFor(x => x.End)
      .GreaterThan(wh => wh.Start)
      .WithMessage("The end working hour value must be greater than the start");

    RuleFor(x => x.Timezone)
      .NotNull().WithMessage("Timezone is required");
  }
}
