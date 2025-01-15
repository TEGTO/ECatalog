using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductApi.Core.Dtos.Endpoints.DeleteProduct;
using ProductApi.Core.Entities;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Endpoints.DeleteProduct.Tests
{
    [TestFixture]
    internal class DeleteProductControllerTests
    {
        private Mock<IProductRepository> repositoryMock;
        private DeleteProductController controller;

        [SetUp]
        public void Setup()
        {
            repositoryMock = new Mock<IProductRepository>();
            controller = new DeleteProductController(repositoryMock.Object);
        }

        [Test]
        public async Task DeleteProductAsync_ProductExists_ReturnsOk()
        {
            // Arrange
            var request = new DeleteProductRequest { Code = "1234-5678" };
            var product = new Product
            {
                Code = "1234-5678",
                Name = "Test Product",
                Price = 100.00m
            };

            repositoryMock
                .Setup(r => r.GetProductByCodeAsync(request.Code, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            repositoryMock
                .Setup(r => r.DeleteProductAsync(product, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await controller.DeleteProductAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkResult>(result);

            repositoryMock.Verify(r => r.GetProductByCodeAsync(request.Code, It.IsAny<CancellationToken>()), Times.Once);
            repositoryMock.Verify(r => r.DeleteProductAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteProductAsync_ProductDoesNotExist_ReturnsOk()
        {
            // Arrange
            var request = new DeleteProductRequest { Code = "1234-5678" };

            repositoryMock
                .Setup(r => r.GetProductByCodeAsync(request.Code, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await controller.DeleteProductAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkResult>(result);

            repositoryMock.Verify(r => r.GetProductByCodeAsync(request.Code, It.IsAny<CancellationToken>()), Times.Once);
            repositoryMock.Verify(r => r.DeleteProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}