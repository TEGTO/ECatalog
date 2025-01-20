using AutoMapper;
using ExceptionHandling;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Core.Dtos.Endpoints.CreateProduct;
using ProductApi.Core.Entities;
using ProductApi.Documentation.Examples.CreateProduct;
using ProductApi.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace ProductApi.Endpoints.CreateProduct
{
    [Route("api/v1/products")]
    [ApiController]
    public class CreateProductController : ControllerBase
    {
        private readonly IProductRepository repository;
        private readonly IMapper mapper;

        public CreateProductController(IProductRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a product",
            Description = "Creates a product using the provided details and returns the product's location and details."
        )]
        [SwaggerRequestExample(typeof(CreateProductRequest), typeof(CreateProductRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(CreateProductResponseExample))]
        [ProducesResponseType(typeof(CreateProductResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateProductResponse>> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var productToCreate = mapper.Map<Product>(request);
            var createdProduct = await repository.CreateProductAsync(productToCreate, cancellationToken);

            var response = mapper.Map<CreateProductResponse>(createdProduct);

            var locationUri = Url.Action(nameof(GetProductByCode), "GetProductByCodeAsync", new { code = response.Code }, Request.Scheme);

            return Created(locationUri, response);
        }
    }
}
