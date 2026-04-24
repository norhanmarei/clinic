using App.Application.DTOs;
using FluentValidation;
namespace App.Application.Validators;
public class GetClinicByNameRequestValidator : AbstractValidator<GetClinicByNameRequest>
{
  private bool BeValidText(string text)
  {
    foreach(char c in text)
    {
      if (char.IsControl(c))
        return false;
    }
    return true;
  }
  public GetClinicByNameRequestValidator()
  {
     RuleFor(x => x.Name)
       .Cascade(CascadeMode.Stop)
       .NotEmpty().WithMessage("Name must not be empty")
       .MaximumLength(200).WithMessage("Name max length is 200")
       .Must(BeValidText).WithMessage("Name must not contain invalid characters");
  }
}
