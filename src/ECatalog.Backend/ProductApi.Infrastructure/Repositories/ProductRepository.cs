using DatabaseControl.Repositories;
using Microsoft.EntityFrameworkCore;
using ProductApi.Core.Dtos.Endpoints.GetProducts;
using ProductApi.Core.Entities;
using ProductApi.Infrastructure.Data;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDatabaseRepository<ProductDbContext> repository;

        public ProductRepository(IDatabaseRepository<ProductDbContext> repository)
        {
            this.repository = repository;
        }

        #region IProductRepository Members

        public async Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken)
        {
            using var dbContext = await repository.GetDbContextAsync(cancellationToken);

            product.Code = GenerateRandomProductCode();
            var createdProduct = await repository.AddAsync(dbContext, product, cancellationToken);

            await repository.SaveChangesAsync(dbContext, cancellationToken);

            return createdProduct;
        }

        public async Task<IEnumerable<GetProductsResponse>> GetProductsAsync(
            string? search = null,
            string? sortBy = null,
            bool descending = false,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            using var dbContext = await repository.GetDbContextAsync(cancellationToken);

            var query = repository.Query<Product>(dbContext);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                query = ApplySorting(query, sortBy, descending);
            }

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var result = query.Select(p => new GetProductsResponse
            {
                Code = p.Code,
                Name = p.Name,
                Price = p.Price
            });

            return await result.ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetProductByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            using var dbContext = await repository.GetDbContextAsync(cancellationToken);

            return await repository
                .Query<Product>(dbContext)
                .FirstOrDefaultAsync(p => p.Code == code, cancellationToken);
        }

        public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            using var dbContext = await repository.GetDbContextAsync(cancellationToken);
            repository.Update(dbContext, product);
            await repository.SaveChangesAsync(dbContext, cancellationToken);
        }

        public async Task DeleteProductAsync(Product product, CancellationToken cancellationToken)
        {
            using var dbContext = await repository.GetDbContextAsync(cancellationToken);
            repository.Remove(dbContext, product);
            await repository.SaveChangesAsync(dbContext, cancellationToken);
        }

        #endregion

        #region Private Helpers

        private static IQueryable<Product> ApplySorting(IQueryable<Product> query, string sortBy, bool descending)
        {
            return sortBy.ToLower() switch
            {
                "name" => descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                "price" => descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                _ => query.OrderBy(p => p.Code)
            };
        }

        private static string GenerateRandomProductCode()
        {
            var random = new Random();
            return $"{random.Next(1000, 10000)}-{random.Next(1000, 10000)}";
        }

        #endregion
    }
}
