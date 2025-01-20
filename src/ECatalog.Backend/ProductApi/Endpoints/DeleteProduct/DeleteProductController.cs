using ExceptionHandling;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Core.Dtos.Endpoints.DeleteProduct;
using ProductApi.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(
            Summary = "Delete a product",
            Description = "Deletes a product based on the provided code. If the product does not exist, returns a 200 OK without performing any operation."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
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
