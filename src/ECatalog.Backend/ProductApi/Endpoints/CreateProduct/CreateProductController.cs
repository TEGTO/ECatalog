using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Core.Dtos.Endpoints.CreateProduct;
using ProductApi.Core.Entities;
using ProductApi.Infrastructure.Repositories;

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
