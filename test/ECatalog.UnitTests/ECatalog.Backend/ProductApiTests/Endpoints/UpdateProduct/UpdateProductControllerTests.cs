using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductApi.Core.Dtos.Endpoints.UpdateProduct;
using ProductApi.Core.Entities;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Endpoints.UpdateProduct.Tests
{
    [TestFixture]
    internal class UpdateProductControllerTests
    {
        private Mock<IProductRepository> repositoryMock;
        private Mock<IMapper> mapperMock;
        private UpdateProductController controller;

        [SetUp]
        public void Setup()
        {
            repositoryMock = new Mock<IProductRepository>();
            mapperMock = new Mock<IMapper>();
            controller = new UpdateProductController(repositoryMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task UpdateProductAsync_ValidRequest_UpdatesProductAndReturnsOk()
        {
            // Arrange
            var request = new UpdateProductRequest
            {
                Code = "1234-5678",
                Name = "UpdatedProduct",
                Description = "UpdatedDescription",
                Price = 99.99m
            };

            var product = new Product
            {
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price
            };

            mapperMock.Setup(m => m.Map<Product>(request)).Returns(product);
            repositoryMock.Setup(r => r.UpdateProductAsync(product, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await controller.UpdateProductAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkResult>(result);

            mapperMock.Verify(m => m.Map<Product>(request), Times.Once);
            repositoryMock.Verify(r => r.UpdateProductAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}