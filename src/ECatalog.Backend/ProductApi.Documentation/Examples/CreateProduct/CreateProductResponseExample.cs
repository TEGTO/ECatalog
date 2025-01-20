using ProductApi.Core.Dtos.Endpoints.CreateProduct;
using Swashbuckle.AspNetCore.Filters;

namespace ProductApi.Documentation.Examples.CreateProduct
{
    public class CreateProductResponseExample : IExamplesProvider<CreateProductResponse>
    {
        public CreateProductResponse GetExamples()
        {
            return new CreateProductResponse
            {
                Code = "EX123",
                Name = "Example Product",
                Description = "An example product description",
                Price = 19.99m
            };
        }
    }
}
