namespace ProductApi.Core.Dtos.Endpoints.CreateProduct
{
    public class CreateProductResponse
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}
