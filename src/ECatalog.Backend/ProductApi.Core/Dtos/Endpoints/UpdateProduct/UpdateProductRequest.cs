namespace ProductApi.Core.Dtos.Endpoints.UpdateProduct
{
    public class UpdateProductRequest
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}
