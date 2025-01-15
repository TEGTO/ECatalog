using ProductApi.Core.Dtos.Endpoints.GetProducts;
using ProductApi.Core.Entities;

namespace ProductApi.Infrastructure.Repositories
{
    public interface IProductRepository
    {
        public Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken);
        public Task DeleteProductAsync(Product product, CancellationToken cancellationToken);
        public Task<Product?> GetProductByCodeAsync(string code, CancellationToken cancellationToken = default);
        public Task<IEnumerable<GetProductsResponse>> GetProductsAsync(string? search = null, string? sortBy = null, bool descending = false, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        public Task UpdateProductAsync(Product product, CancellationToken cancellationToken = default);
    }
}