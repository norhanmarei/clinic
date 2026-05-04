using FluentValidation;
namespace App.Application.Validators;
public class GetClinicByNameRequestValidator : AbstractValidator<string>
{
  public GetClinicByNameRequestValidator()
  {
     RuleFor(x => x)
       .Cascade(CascadeMode.Stop)
       .NotEmpty().WithMessage("Name must not be empty")
       .MaximumLength(200).WithMessage("Name max length is 200");
  }
}
