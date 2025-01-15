using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Core.Dtos.Endpoints.UpdateProduct;
using ProductApi.Core.Entities;
using ProductApi.Infrastructure.Repositories;

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
        public async Task<IActionResult> UpdateProductAsync(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var productToUpdate = mapper.Map<Product>(request);
            await repository.UpdateProductAsync(productToUpdate, cancellationToken);

            return Ok();
        }
    }
}
