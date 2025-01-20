using AutoMapper;
using ExceptionHandling;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Core.Dtos.Endpoints.UpdateProduct;
using ProductApi.Core.Entities;
using ProductApi.Documentation.Examples.UpdateProduct;
using ProductApi.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace ProductApi.Endpoints.UpdateProduct
{
    [Route("api/v1/products")]
    [ApiController]
    public class UpdateProductController : ControllerBase
    {
        private readonly IProductRepository repository;
        private readonly IMapper mapper;

        public UpdateProductController(IProductRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpPut]
        [SwaggerOperation(
            Summary = "Update an existing product",
            Description = "Updates the details of an existing product based on the provided request. Returns 200 OK if successful."
        )]
        [SwaggerRequestExample(typeof(UpdateProductRequest), typeof(UpdateProductRequestExample))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProductAsync(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var productToUpdate = mapper.Map<Product>(request);
            await repository.UpdateProductAsync(productToUpdate, cancellationToken);

            return Ok();
        }
    }
}
