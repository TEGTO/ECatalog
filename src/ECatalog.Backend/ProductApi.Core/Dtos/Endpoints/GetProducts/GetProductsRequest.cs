namespace ProductApi.Core.Dtos.Endpoints.GetProducts
{
    public class GetProductsRequest
    {
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public bool Descending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
