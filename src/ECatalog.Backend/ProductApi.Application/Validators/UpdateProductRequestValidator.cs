using FluentValidation;
using ProductApi.Core;
using ProductApi.Core.Dtos.Endpoints.UpdateProduct;

namespace ProductApi.Application.Validators
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Code)
                .NotNull()
                .NotEmpty()
                .Matches(EntitiesRegex.ProductRegex)
                .WithMessage("Code must be in the format {XXXX-XXXX}, where X is a digit.");

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .Length(3, 100)
                .WithMessage("Name must be between 3 and 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .PrecisionScale(18, 2, true)
                .WithMessage("Price must be greater than 0 and have up to two decimal places.");
        }
    }
}
