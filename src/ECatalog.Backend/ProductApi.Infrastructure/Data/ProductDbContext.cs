using Microsoft.EntityFrameworkCore;
using ProductApi.Core.Entities;

namespace ProductApi.Infrastructure.Data
{
    public class ProductDbContext : DbContext
    {
        public virtual DbSet<Product> ServerSlots { get; set; }

        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
