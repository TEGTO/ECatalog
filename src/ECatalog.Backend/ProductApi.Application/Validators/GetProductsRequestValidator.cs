using FluentValidation;
using ProductApi.Core.Dtos.Endpoints.GetProducts;

namespace ProductApi.Application.Validators
{
    public class GetProductsRequestValidator : AbstractValidator<GetProductsRequest>
    {
        public GetProductsRequestValidator()
        {
            RuleFor(request => request.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(request => request.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0.");

            RuleFor(request => request.SortBy)
                .MaximumLength(50)
                .WithMessage("SortBy cannot exceed 50 characters.");
        }
    }
}
