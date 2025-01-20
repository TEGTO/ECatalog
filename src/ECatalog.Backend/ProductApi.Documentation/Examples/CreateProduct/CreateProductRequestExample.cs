using ProductApi.Core.Dtos.Endpoints.CreateProduct;
using Swashbuckle.AspNetCore.Filters;

namespace ProductApi.Documentation.Examples.CreateProduct
{
    public class CreateProductRequestExample : IExamplesProvider<CreateProductRequest>
    {
        public CreateProductRequest GetExamples()
        {
            return new CreateProductRequest
            {
                Name = "Example Product",
                Description = "An example product description",
                Price = 19.99m
            };
        }
    }
}
