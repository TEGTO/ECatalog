using Microsoft.AspNetCore.Mvc;

namespace ProductApi.Core.Dtos.Endpoints.GetProductByCode
{
    public class GetProductByCodeRequest
    {
        [FromRoute]
        public string Code { get; set; } = string.Empty;
    }
}
