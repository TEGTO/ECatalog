using ProductApi.Core.Dtos.Endpoints.GetProductByCode;
using Swashbuckle.AspNetCore.Filters;

namespace ProductApi.Documentation.Examples.GetProductByCode
{
    public class GetProductByCodeResponseExample : IExamplesProvider<GetProductByCodeResponse>
    {
        public GetProductByCodeResponse GetExamples()
        {
            return new GetProductByCodeResponse
            {
                Code = "EX123",
                Name = "Example Product",
                Description = "An example product description",
                Price = 19.99m
            };
        }
    }
}
