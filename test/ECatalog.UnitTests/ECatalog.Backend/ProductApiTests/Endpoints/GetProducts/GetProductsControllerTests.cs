using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductApi.Core.Dtos.Endpoints.GetProducts;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Endpoints.GetProducts.Tests
{
    [TestFixture]
    internal class GetProductsControllerTests
    {
        private Mock<IProductRepository> repositoryMock;
        private GetProductsController controller;

        [SetUp]
        public void Setup()
        {
            repositoryMock = new Mock<IProductRepository>();
            controller = new GetProductsController(repositoryMock.Object);
        }

        [Test]
        public async Task GetContractsAsync_ValidRequest_ReturnsOkWithProducts()
        {
            // Arrange
            var request = new GetProductsRequest
            {
                Search = "Test",
                SortBy = "Name",
                Descending = false,
                PageNumber = 1,
                PageSize = 10
            };

            var products = new List<GetProductsResponse>
            {
                new GetProductsResponse { Code = "1234-5678", Name = "Product1", Price = 100.00m },
                new GetProductsResponse { Code = "5678-1234", Name = "Product2", Price = 200.00m }
            };

            repositoryMock
                .Setup(r => r.GetProductsAsync(
                    request.Search,
                    request.SortBy,
                    request.Descending,
                    request.PageNumber,
                    request.PageSize,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // Act
            var result = await controller.GetContractsAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
            Assert.IsInstanceOf<IEnumerable<GetProductsResponse>>(okResult.Value);

            var responseData = okResult.Value as IEnumerable<GetProductsResponse>;
            Assert.That(responseData?.Count(), Is.EqualTo(2));

            repositoryMock.Verify(r => r.GetProductsAsync(
                request.Search,
                request.SortBy,
                request.Descending,
                request.PageNumber,
                request.PageSize,
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetContractsAsync_NoProductsFound_ReturnsOkWithEmptyList()
        {
            // Arrange
            var request = new GetProductsRequest
            {
                Search = "NonExistentProduct",
                PageNumber = 1,
                PageSize = 10
            };

            var products = new List<GetProductsResponse>();

            repositoryMock
                .Setup(r => r.GetProductsAsync(
                    request.Search,
                    null,
                    false,
                    request.PageNumber,
                    request.PageSize,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // Act
            var result = await controller.GetContractsAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
            Assert.IsInstanceOf<IEnumerable<GetProductsResponse>>(okResult.Value);

            var responseData = okResult.Value as IEnumerable<GetProductsResponse>;
            Assert.That(responseData?.Count(), Is.EqualTo(0));

            repositoryMock.Verify(r => r.GetProductsAsync(
                request.Search,
                null,
                false,
                request.PageNumber,
                request.PageSize,
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}