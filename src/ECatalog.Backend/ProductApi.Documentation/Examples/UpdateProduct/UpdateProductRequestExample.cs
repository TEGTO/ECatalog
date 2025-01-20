using ProductApi.Core.Dtos.Endpoints.UpdateProduct;
using Swashbuckle.AspNetCore.Filters;

namespace ProductApi.Documentation.Examples.UpdateProduct
{
    public class UpdateProductRequestExample : IExamplesProvider<UpdateProductRequest>
    {
        public UpdateProductRequest GetExamples()
        {
            return new UpdateProductRequest
            {
                Code = "P123",
                Name = "Updated Product Name",
                Description = "Updated product description",
                Price = 199.99m
            };
        }
    }
}
