using Microsoft.AspNetCore.Mvc;
using ProductApi.Core.Dtos.Endpoints.DeleteProduct;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Endpoints.DeleteProduct
{
    [Route("api/v1/products")]
    [ApiController]
    public class DeleteProductController : ControllerBase
    {
        private readonly IProductRepository repository;

        public DeleteProductController(IProductRepository repository)
        {
            this.repository = repository;
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] DeleteProductRequest request, CancellationToken cancellationToken)
        {
            var product = await repository.GetProductByCodeAsync(request.Code, cancellationToken);

            if (product != null)
            {
                await repository.DeleteProductAsync(product, cancellationToken);
            }

            return Ok();
        }
    }
}
