using Microsoft.AspNetCore.Mvc;

namespace ProductApi.Core.Dtos.Endpoints.DeleteProduct
{
    public class DeleteProductRequest
    {
        [FromRoute]
        public string Code { get; set; } = string.Empty;
    }
}
