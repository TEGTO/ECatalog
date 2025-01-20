using ProductApi.Core.Dtos.Endpoints.GetProducts;
using Swashbuckle.AspNetCore.Filters;

namespace ProductApi.Documentation.Examples.GetProducts
{
    public class GetProductsResponseExample : IExamplesProvider<IEnumerable<GetProductsResponse>>
    {
        public IEnumerable<GetProductsResponse> GetExamples()
        {
            return new List<GetProductsResponse>
            {
                new GetProductsResponse
                {
                    Code = "P001",
                    Name = "Example Product 1",
                    Price = 99.99m
                },
                new GetProductsResponse
                {
                    Code = "P002",
                    Name = "Example Product 2",
                    Price = 149.99m
                }
            };
        }
    }
}
