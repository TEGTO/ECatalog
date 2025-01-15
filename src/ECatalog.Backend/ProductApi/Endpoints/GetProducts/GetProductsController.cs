using Microsoft.AspNetCore.Mvc;
using ProductApi.Core.Dtos.Endpoints.GetProducts;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Endpoints.GetProducts
{
    [Route("api/v1/products")]
    [ApiController]
    public class GetProductsController : ControllerBase
    {
        private readonly IProductRepository repository;

        public GetProductsController(IProductRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetProductsResponse>>> GetContractsAsync(
            [FromQuery] GetProductsRequest request,
            CancellationToken cancellationToken = default)
        {
            var products = await repository.GetProductsAsync(
                request.Search,
                request.SortBy,
                request.Descending,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            return Ok(products);
        }
    }
}
