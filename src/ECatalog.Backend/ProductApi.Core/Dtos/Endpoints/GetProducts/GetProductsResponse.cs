namespace ProductApi.Core.Dtos.Endpoints.GetProducts
{
    public class GetProductsResponse
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
