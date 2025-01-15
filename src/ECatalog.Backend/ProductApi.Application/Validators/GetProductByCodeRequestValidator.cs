using FluentValidation;
using ProductApi.Core;
using ProductApi.Core.Dtos.Endpoints.GetProductByCode;

namespace ProductApi.Application.Validators
{
    public class GetProductByCodeRequestValidator : AbstractValidator<GetProductByCodeRequest>
    {
        public GetProductByCodeRequestValidator()
        {
            RuleFor(x => x.Code)
                .NotNull()
                .NotEmpty()
                .Matches(EntitiesRegex.ProductRegex)
                .WithMessage("Code must be in the format {XXXX-XXXX}, where X is a digit.");
        }
    }
}
