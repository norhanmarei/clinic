using App.Application.Common.Requests;
using FluentValidation;

namespace App.Application.Validators;
public class GetPagedResultValidator: AbstractValidator<GetPagedResultRequest>
{
  public GetPagedResultValidator()
  {
     RuleFor(x => x.Page)
       .GreaterThanOrEqualTo(1).WithMessage("Page must be greater than or equal to 1");
     RuleFor(x => x.PageSize)
       .GreaterThanOrEqualTo(1).WithMessage("Page size must be greater than or equal to 1");
  }
}
