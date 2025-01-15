using DatabaseControl.Repositories;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using ProductApi.Core.Entities;
using ProductApi.Infrastructure.Data;

namespace ProductApi.Infrastructure.Repositories.Tests
{
    [TestFixture]
    internal class ProductRepositoryTests
    {
        private Mock<IDatabaseRepository<ProductDbContext>> repositoryMock;
        private ProductRepository productRepository;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IDatabaseRepository<ProductDbContext>>();

            repositoryMock.Setup(x => x.GetDbContextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => null!);

            productRepository = new ProductRepository(repositoryMock.Object);
            cancellationToken = new CancellationToken();
        }

        private static Mock<DbSet<T>> GetDbSetMock<T>(List<T> data) where T : class
        {
            return data.AsQueryable().BuildMockDbSet();
        }

        [Test]
        public async Task CreateProductAsync_ValidProduct_CreatesProduct()
        {
            // Arrange
            var product = new Product { Name = "TestProduct", Price = 100 };

            repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<ProductDbContext>(), product, cancellationToken))
                .ReturnsAsync(product);

            // Act
            var createdProduct = await productRepository.CreateProductAsync(product, cancellationToken);

            // Assert
            Assert.IsNotNull(createdProduct);
            Assert.That(createdProduct.Code, Is.Not.Null.Or.Empty);
            Assert.That(createdProduct.Name, Is.EqualTo(product.Name));

            repositoryMock.Verify(repo => repo.GetDbContextAsync(cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ProductDbContext>(), product, cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<ProductDbContext>(), cancellationToken), Times.Once);
        }

        [Test]
        public async Task GetProductsAsync_ReturnsEmptyList_WhenNoProductsExist()
        {
            // Arrange
            var dbSetMock = GetDbSetMock(new List<Product>());

            repositoryMock.Setup(repo => repo.Query<Product>(It.IsAny<ProductDbContext>())).Returns(dbSetMock.Object);

            // Act
            var result = await productRepository.GetProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetProductsAsync_ReturnsFilteredResults_WhenSearchIsProvided()
        {
            // Arrange
            var dbSetMock = GetDbSetMock(new List<Product>
            {
                new Product { Name = "Laptop", Code = "P001", Price = 1000 },
                new Product { Name = "Mouse", Code = "P002", Price = 50 }
            });

            repositoryMock.Setup(repo => repo.Query<Product>(It.IsAny<ProductDbContext>())).Returns(dbSetMock.Object);

            // Act
            var result = await productRepository.GetProductsAsync(search: "Laptop");

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Laptop"));
        }

        [Test]
        public async Task GetProductsAsync_ReturnsSortedResults_WhenSortByIsProvided()
        {
            // Arrange
            var dbSetMock = GetDbSetMock(new List<Product>
            {
                new Product { Name = "Laptop", Code = "P001", Price = 1000 },
                new Product { Name = "Mouse", Code = "P002", Price = 50 },
                new Product { Name = "Keyboard", Code = "P003", Price = 80 }
            });

            repositoryMock.Setup(repo => repo.Query<Product>(It.IsAny<ProductDbContext>())).Returns(dbSetMock.Object);

            // Act
            var result = await productRepository.GetProductsAsync(sortBy: "Price", descending: true);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.First().Name, Is.EqualTo("Laptop"));
        }

        [Test]
        public async Task GetProductsAsync_HandlesPaginationCorrectly()
        {
            // Arrange
            var products = new List<Product>();
            for (int i = 1; i <= 25; i++)
            {
                products.Add(new Product { Name = $"Product {i}", Code = $"P{i:D3}", Price = i * 10 });
            }

            repositoryMock.Setup(repo => repo.Query<Product>(It.IsAny<ProductDbContext>()))
                .Returns(GetDbSetMock(products).Object);

            // Act
            var result = await productRepository.GetProductsAsync(pageNumber: 2, pageSize: 10);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(10));
            Assert.That(result.First().Name, Is.EqualTo("Product 11")); // Second page starts at Product 11
        }

        [Test]
        public async Task GetProductByCodeAsync_ValidCode_ReturnsProduct()
        {
            // Arrange
            var product = new Product { Code = "1234-5678", Name = "TestProduct", Price = 100 };
            var dbSetMock = GetDbSetMock(new List<Product> { product });

            repositoryMock.Setup(repo => repo.Query<Product>(It.IsAny<ProductDbContext>())).Returns(dbSetMock.Object);

            // Act
            var result = await productRepository.GetProductByCodeAsync("1234-5678", cancellationToken);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(product.Name));
            repositoryMock.Verify(repo => repo.GetDbContextAsync(cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.Query<Product>(It.IsAny<ProductDbContext>()), Times.Once);
        }

        [Test]
        public async Task UpdateProductAsync_ValidProduct_UpdatesProduct()
        {
            // Arrange
            var product = new Product { Code = "1234-5678", Name = "UpdatedProduct", Price = 200 };

            // Act
            await productRepository.UpdateProductAsync(product, cancellationToken);

            // Assert
            repositoryMock.Verify(repo => repo.GetDbContextAsync(cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.Update(It.IsAny<ProductDbContext>(), product), Times.Once);
            repositoryMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<ProductDbContext>(), cancellationToken), Times.Once);
        }

        [Test]
        public async Task DeleteProductAsync_ValidProduct_DeletesProduct()
        {
            // Arrange
            var product = new Product { Code = "1234-5678", Name = "ProductToDelete", Price = 100 };

            // Act
            await productRepository.DeleteProductAsync(product, cancellationToken);

            // Assert
            repositoryMock.Verify(repo => repo.GetDbContextAsync(cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.Remove(It.IsAny<ProductDbContext>(), product), Times.Once);
            repositoryMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<ProductDbContext>(), cancellationToken), Times.Once);
        }
    }
}