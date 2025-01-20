using AutoMapper;
using ExceptionHandling;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Core.Dtos.Endpoints.GetProductByCode;
using ProductApi.Documentation.Examples.GetProductByCode;
using ProductApi.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace ProductApi.Endpoints.GetProductByCode
{
    [Route("api/v1/products")]
    [ApiController]
    public class GetProductByCodeController : ControllerBase
    {
        private readonly IProductRepository repository;
        private readonly IMapper mapper;

        public GetProductByCodeController(IProductRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet("{code}")]
        [SwaggerOperation(
            Summary = "Get the product by code",
            Description = "Finds the product by the code. If the product does not exist, returns a 404 NotFound."
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetProductByCodeResponseExample))]
        [ProducesResponseType(typeof(GetProductByCodeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetProductByCodeResponse>> GetProductByCodeAsync(
            [FromRoute] GetProductByCodeRequest request, CancellationToken cancellationToken)
        {
            var product = await repository.GetProductByCodeAsync(request.Code, cancellationToken);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<GetProductByCodeResponse>(product));
        }
    }
}
