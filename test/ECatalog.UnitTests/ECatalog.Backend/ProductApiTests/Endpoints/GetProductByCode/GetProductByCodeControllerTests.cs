using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductApi.Core.Dtos.Endpoints.GetProductByCode;
using ProductApi.Core.Entities;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Endpoints.GetProductByCode.Tests
{
    [TestFixture]
    internal class GetProductByCodeControllerTests
    {
        private Mock<IProductRepository> repositoryMock;
        private Mock<IMapper> mapperMock;
        private GetProductByCodeController controller;

        [SetUp]
        public void Setup()
        {
            repositoryMock = new Mock<IProductRepository>();
            mapperMock = new Mock<IMapper>();
            controller = new GetProductByCodeController(repositoryMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task GetProductByCodeAsync_ProductExists_ReturnsOk()
        {
            // Arrange
            var request = new GetProductByCodeRequest { Code = "1234-5678" };
            var product = new Product
            {
                Code = "1234-5678",
                Name = "Test Product",
                Description = "This is a test product",
                Price = 100.00m
            };
            var response = new GetProductByCodeResponse
            {
                Code = "1234-5678",
                Name = "Test Product",
                Description = "This is a test product",
                Price = 100.00m
            };

            repositoryMock
                .Setup(r => r.GetProductByCodeAsync(request.Code, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            mapperMock
                .Setup(m => m.Map<GetProductByCodeResponse>(product))
                .Returns(response);

            // Act
            var result = await controller.GetProductByCodeAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
            Assert.IsInstanceOf<GetProductByCodeResponse>(okResult.Value);

            var responseData = okResult.Value as GetProductByCodeResponse;
            Assert.That(responseData?.Code, Is.EqualTo("1234-5678"));
            Assert.That(responseData?.Name, Is.EqualTo("Test Product"));
            Assert.That(responseData?.Description, Is.EqualTo("This is a test product"));
            Assert.That(responseData?.Price, Is.EqualTo(100.00m));

            repositoryMock.Verify(r => r.GetProductByCodeAsync(request.Code, It.IsAny<CancellationToken>()), Times.Once);
            mapperMock.Verify(m => m.Map<GetProductByCodeResponse>(product), Times.Once);
        }

        [Test]
        public async Task GetProductByCodeAsync_ProductDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var request = new GetProductByCodeRequest { Code = "1234-5678" };

            repositoryMock
                .Setup(r => r.GetProductByCodeAsync(request.Code, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await controller.GetProductByCodeAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);

            repositoryMock.Verify(r => r.GetProductByCodeAsync(request.Code, It.IsAny<CancellationToken>()), Times.Once);
            mapperMock.Verify(m => m.Map<GetProductByCodeResponse>(It.IsAny<Product>()), Times.Never);
        }
    }
}