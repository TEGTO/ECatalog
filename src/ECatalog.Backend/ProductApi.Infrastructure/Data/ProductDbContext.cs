using Microsoft.EntityFrameworkCore;
using ProductApi.Core;
using ProductApi.Core.Entities;

namespace ProductApi.Infrastructure.Data
{
    public class ProductDbContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }

        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_products_Code_Format", $"code ~ '{EntitiesRegex.ProductRegex}'");
                });
            });
        }
    }
}
