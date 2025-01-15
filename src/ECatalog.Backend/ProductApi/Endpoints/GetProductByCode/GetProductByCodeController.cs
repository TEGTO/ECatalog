using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Core.Dtos.Endpoints.GetProductByCode;
using ProductApi.Infrastructure.Repositories;

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
