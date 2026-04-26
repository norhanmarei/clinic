using App.Application.DTOs;
using FluentValidation;
namespace App.Application.Validators;
public class GetClinicByNameRequestValidator : AbstractValidator<GetClinicByNameRequest>
{
  public GetClinicByNameRequestValidator()
  {
     RuleFor(x => x.Name)
       .Cascade(CascadeMode.Stop)
       .NotEmpty().WithMessage("Name must not be empty")
       .MaximumLength(200).WithMessage("Name max length is 200");
  }
}
