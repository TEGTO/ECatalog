using AutoMapper;
using ProductApi.Core.Dtos.Endpoints.CreateProduct;
using ProductApi.Core.Dtos.Endpoints.GetProductByCode;
using ProductApi.Core.Dtos.Endpoints.GetProducts;
using ProductApi.Core.Dtos.Endpoints.UpdateProduct;
using ProductApi.Core.Entities;

namespace ProductApi.Application.Tests
{
    [TestFixture]
    internal class AutoMapperProfileTests
    {
        private IMapper mapper;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            mapper = config.CreateMapper();
        }

        [Test]
        public void Map_CreateProductRequest_To_Product()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "ProductName",
                Description = "ProductDescription",
                Price = 123.45m
            };

            // Act
            var result = mapper.Map<Product>(request);

            // Assert
            Assert.That(result.Name, Is.EqualTo(request.Name));
            Assert.That(result.Description, Is.EqualTo(request.Description));
            Assert.That(result.Price, Is.EqualTo(request.Price));
        }

        [Test]
        public void Map_Product_To_CreateProductResponse()
        {
            // Arrange
            var product = new Product
            {
                Code = "1234-5678",
                Name = "ProductName",
                Description = "ProductDescription",
                Price = 123.45m
            };

            // Act
            var result = mapper.Map<CreateProductResponse>(product);

            // Assert
            Assert.That(result.Code, Is.EqualTo(product.Code));
            Assert.That(result.Name, Is.EqualTo(product.Name));
            Assert.That(result.Description, Is.EqualTo(product.Description));
            Assert.That(result.Price, Is.EqualTo(product.Price));
        }

        [Test]
        public void Map_Product_To_GetProductByCodeResponse()
        {
            // Arrange
            var product = new Product
            {
                Code = "1234-5678",
                Name = "ProductName",
                Description = "ProductDescription",
                Price = 123.45m
            };

            // Act
            var result = mapper.Map<GetProductByCodeResponse>(product);

            // Assert
            Assert.That(result.Code, Is.EqualTo(product.Code));
            Assert.That(result.Name, Is.EqualTo(product.Name));
            Assert.That(result.Description, Is.EqualTo(product.Description));
            Assert.That(result.Price, Is.EqualTo(product.Price));
        }

        [Test]
        public void Map_Product_To_GetProductsResponse()
        {
            // Arrange
            var product = new Product
            {
                Code = "1234-5678",
                Name = "ProductName",
                Price = 123.45m
            };

            // Act
            var result = mapper.Map<GetProductsResponse>(product);

            // Assert
            Assert.That(result.Code, Is.EqualTo(product.Code));
            Assert.That(result.Name, Is.EqualTo(product.Name));
            Assert.That(result.Price, Is.EqualTo(product.Price));
        }

        [Test]
        public void Map_UpdateProductRequest_To_Product()
        {
            // Arrange
            var request = new UpdateProductRequest
            {
                Code = "1234-5678",
                Name = "UpdatedProduct",
                Description = "UpdatedDescription",
                Price = 543.21m
            };

            // Act
            var result = mapper.Map<Product>(request);

            // Assert
            Assert.That(result.Code, Is.EqualTo(request.Code));
            Assert.That(result.Name, Is.EqualTo(request.Name));
            Assert.That(result.Description, Is.EqualTo(request.Description));
            Assert.That(result.Price, Is.EqualTo(request.Price));
        }
    }
}