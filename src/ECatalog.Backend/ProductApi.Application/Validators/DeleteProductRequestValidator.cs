using FluentValidation;
using ProductApi.Core;
using ProductApi.Core.Dtos.Endpoints.DeleteProduct;

namespace ProductApi.Application.Validators
{
    public class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequest>
    {
        public DeleteProductRequestValidator()
        {
            RuleFor(x => x.Code)
                .NotNull()
                .NotEmpty()
                .Matches(EntitiesRegex.ProductRegex)
                .WithMessage("Code must be in the format {XXXX-XXXX}, where X is a digit.");
        }
    }
}
