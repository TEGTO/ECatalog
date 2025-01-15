using FluentValidation;
using ProductApi.Core.Dtos.Endpoints.CreateProduct;

namespace ProductApi.Application.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
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
