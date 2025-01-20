using ExceptionHandling;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Core.Dtos.Endpoints.GetProducts;
using ProductApi.Documentation.Examples.GetProducts;
using ProductApi.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

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
        [SwaggerOperation(
            Summary = "Get a list of products",
            Description = "Retrieves a paginated list of products based on optional search and sorting parameters."
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetProductsResponseExample))]
        [ProducesResponseType(typeof(IEnumerable<GetProductsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
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
