using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using ProductApi.Core.Dtos.Endpoints.CreateProduct;
using ProductApi.Core.Entities;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Endpoints.CreateProduct.Tests
{
    [TestFixture]
    internal class CreateProductControllerTests
    {
        private Mock<IProductRepository> repositoryMock;
        private Mock<IMapper> mapperMock;
        private CreateProductController controller;

        [SetUp]
        public void Setup()
        {
            repositoryMock = new Mock<IProductRepository>();
            mapperMock = new Mock<IMapper>();

            controller = new CreateProductController(repositoryMock.Object, mapperMock.Object);
            controller.Url = new Mock<IUrlHelper>().Object;
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Test]
        public async Task CreateProductAsync_ValidRequest_ReturnsCreatedResult()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 100.00m
            };

            var productToCreate = new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 100.00m,
                Code = "1234-5678"
            };

            var createdProduct = new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 100.00m,
                Code = "1234-5678"
            };

            var expectedResponse = new CreateProductResponse
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 100.00m,
                Code = "1234-5678"
            };

            mapperMock.Setup(m => m.Map<Product>(request)).Returns(productToCreate);
            repositoryMock.Setup(r => r.CreateProductAsync(productToCreate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdProduct);
            mapperMock.Setup(m => m.Map<CreateProductResponse>(createdProduct)).Returns(expectedResponse);

            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.Setup(u => u.Action(It.IsAny<UrlActionContext>()))
                .Returns("http://localhost/api/v1/products/1234-5678");
            controller.Url = urlHelperMock.Object;

            // Act
            var result = await controller.CreateProductAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CreatedResult>(result.Result);

            var createdResult = result.Result as CreatedResult;
            Assert.IsNotNull(createdResult);
            Assert.That(createdResult.Location, Is.EqualTo("http://localhost/api/v1/products/1234-5678"));

            var response = createdResult.Value as CreateProductResponse;
            Assert.IsNotNull(response);
            Assert.That(response.Code, Is.EqualTo(expectedResponse.Code));
            Assert.That(response.Name, Is.EqualTo(expectedResponse.Name));
            Assert.That(response.Description, Is.EqualTo(expectedResponse.Description));
            Assert.That(response.Price, Is.EqualTo(expectedResponse.Price));
        }
    }
}