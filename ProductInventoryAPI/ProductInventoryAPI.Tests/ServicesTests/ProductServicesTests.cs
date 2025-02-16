// <copyright file="ProductServicesTests.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

namespace ProductInventoryAPI.Tests.ServicesTests
{
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using Moq;
    using ProductInventoryAPI.AutoMapperProfiles;
    using ProductInventoryAPI.Dtos;
    using ProductInventoryAPI.Helper;
    using ProductInventoryAPI.Repositories;
    using ProductInventoryAPI.Services;
    using ProductInventoryAPI.Services.Exceptions;
    using Xunit;

    public class ProductServicesTests
    {
        [Fact]
        public async Task GetAllProductsAsync_Success()
        {
            // Arrange
            var productDbContext = CreateDatabaseWithTestProducts();
            var mockLogger = new Mock<ILogger<ProductService>>();
            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);
            var productService = new ProductService(
                productDbContext, CreateMapper(), mockLogger.Object, mockProductIdGenerator.Object);

            // Act
            var productsData = await productService.GetAllProductsAsync();

            // Assert
            Assert.Single(productsData);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldLogWarningWhenNoProducts()
        {
            // Arrange
            var productDbContext = CreateEmptyDatabase();
            var mockLogger = new Mock<ILogger<ProductService>>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<IEnumerable<ProductDto>>(It.IsAny<IEnumerable<Product>>()))
                      .Returns(new List<ProductDto>());

            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);
            var productService = new ProductService(
                productDbContext, CreateMapper(), mockLogger.Object, mockProductIdGenerator.Object);

            // Act
            var productsData = await productService.GetAllProductsAsync();

            // Assert
            Assert.Empty(productsData);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldLogErrorOnException()
        {
            // Arrange
            var mockContext = new Mock<ProductDbContext>();
            var mockLogger = new Mock<ILogger<ProductService>>();
            var mockMapper = new Mock<IMapper>();
            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);
            var productService = new ProductService(
                mockContext.Object, mockMapper.Object, mockLogger.Object, mockProductIdGenerator.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DataAccessException>(() => productService.GetAllProductsAsync());
            Assert.Equal("Failed to fetch product data.", exception.Message);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct()
        {
            // Arrange
            var productDbContext = CreateDatabaseWithTestProducts();
            var mockLogger = new Mock<ILogger<ProductService>>();
            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);

            // Act
            var productService = new ProductService(
                     productDbContext, CreateMapper(), mockLogger.Object, mockProductIdGenerator.Object);
            var productData = productService.GetProductByIdAsync(1);

            // Assert
            Assert.Equal(1, productData.Id);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldLogErrorOnException()
        {
            // Arrange
            var mockContext = new Mock<ProductDbContext>();
            var mockLogger = new Mock<ILogger<ProductService>>();
            var mockMapper = new Mock<IMapper>();
            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);
            var productService = new ProductService(
                mockContext.Object, mockMapper.Object, mockLogger.Object, mockProductIdGenerator.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DataAccessException>(() => productService.GetProductByIdAsync(1));
            Assert.Equal("Failed to fetch product data.", exception.Message);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldLogErrorOnNotFoundException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductService>>();
            var mockMapper = new Mock<IMapper>();

            // Create a context with no product having ID 1
            var productDbContext = CreateDatabaseWithTestProducts();
            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);
            var productService = new ProductService(
                productDbContext, CreateMapper(), mockLogger.Object, mockProductIdGenerator.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => productService.GetProductByIdAsync(10));
            Assert.Equal("Product with ID 10 not found.", exception.Message);
        }

        [Fact]
        public async Task AddProductAsync_ShouldReturnProductDto()
        {
            // Arrange
            var createDto = new ProductUpsertDto
            {
                // Initialize with test data
                Name = "Test Product",
                Price = 100,
                Description = "Test Description",
            };
            var productDbContext = CreateDatabaseWithTestProducts();
            var mockLogger = new Mock<ILogger<ProductService>>();
            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);
            var productService = new ProductService(
                productDbContext, CreateMapper(), mockLogger.Object, mockProductIdGenerator.Object);

            // Act
            var result = await productService.AddProductAsync(createDto);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddProductAsync_ShouldLogErrorOnException()
        {
            // Arrange
            var createDto = new ProductUpsertDto
            {
                // Initialize with test data
                Name = "Test Product",
                Price = 100,
                Description = "Test Description",
            };
            var mockContext = new Mock<ProductDbContext>();
            var mockLogger = new Mock<ILogger<ProductService>>();
            var mockMapper = new Mock<IMapper>();
            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);

            var productService = new ProductService(
                mockContext.Object, mockMapper.Object, mockLogger.Object, mockProductIdGenerator.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DataAccessException>(() => productService.AddProductAsync(createDto));
            Assert.Equal("Failed to add product.", exception.Message);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldReturnProductDto()
        {
            // Arrange
            var updateDto = new ProductUpsertDto
            {
                // Initialize with test data
                Name = "Test Product",
                Price = 100,
                Description = "Test Description",
            };
            var productDbContext = CreateDatabaseWithTestProducts();
            var mockLogger = new Mock<ILogger<ProductService>>();
            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);
            var productService = new ProductService(
                productDbContext, CreateMapper(), mockLogger.Object, mockProductIdGenerator.Object);

            // Act
            var result = await productService.UpdateProductAsync(1, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateDto.Name, result.Name);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldLogErrorOnException()
        {
            // Arrange
            var updateDto = new ProductUpsertDto
            {
                // Initialize with test data
                Name = "Test Product",
                Price = 100,
                Description = "Test Description",
            };
            var mockContext = new Mock<ProductDbContext>();
            var mockLogger = new Mock<ILogger<ProductService>>();
            var mockMapper = new Mock<IMapper>();
            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);
            var productService = new ProductService(
                mockContext.Object, CreateMapper(), mockLogger.Object, mockProductIdGenerator.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DataAccessException>(() => productService.UpdateProductAsync(1, updateDto));
            Assert.Equal("Failed to update product.", exception.Message);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldLogErrorOnNotFoundException()
        {
            // Arrange
            var updateDto = new ProductUpsertDto
            {
                // Initialize with test data
                Name = "Test Product",
                Price = 100,
                Description = "Test Description",
            };
            var mockLogger = new Mock<ILogger<ProductService>>();

            // Create a context with no product having ID 1
            var productDbContext = CreateDatabaseWithTestProducts();

            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);
            var productService = new ProductService(
                productDbContext, CreateMapper(), mockLogger.Object, mockProductIdGenerator.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => productService.UpdateProductAsync(10, updateDto));
            Assert.Equal("Product with ID 10 not found.", exception.Message);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldDeleteProduct()
        {
            // Arrange
            var productDbContext = CreateDatabaseWithTestProducts();
            var mockLogger = new Mock<ILogger<ProductService>>();

            // Act
            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);
            var productService = new ProductService(
                productDbContext, CreateMapper(), mockLogger.Object, mockProductIdGenerator.Object);
            var productData = productService.DeleteProductAsync(1);

            // Assert
            Assert.Equal(2, productData.Id);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldLogErrorOnException()
        {
            // Arrange
            var mockContext = new Mock<ProductDbContext>();
            var mockLogger = new Mock<ILogger<ProductService>>();

            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);
            var productService = new ProductService(
                mockContext.Object, CreateMapper(), mockLogger.Object, mockProductIdGenerator.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DataAccessException>(() => productService.DeleteProductAsync(1));
            Assert.Equal("Failed to delete product.", exception.Message);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldLogErrorOnNotFoundException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductService>>();

            // Create a context with no product having ID 1
            var productDbContext = CreateDatabaseWithTestProducts();

            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);
            var productService = new ProductService(
                productDbContext, CreateMapper(), mockLogger.Object, mockProductIdGenerator.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => productService.DeleteProductAsync(10));
            Assert.Equal("Product with ID 10 not found.", exception.Message);
        }

        [Fact]
        public async Task DecrementStockAsync_ShouldDecrementStock_WhenEnoughStockAvailable()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product", StockAvailable = 15 };
            var productDbContext = CreateDatabaseWithTestProducts();
            var mockLogger = new Mock<ILogger<ProductService>>();
            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);

            // Act
            var productService = new ProductService(
                productDbContext, CreateMapper(), mockLogger.Object, mockProductIdGenerator.Object);

            // Act
            await productService.DecrementStockAsync(1, 10);

            // Assert
            Assert.Equal(15, product.StockAvailable);
        }

        [Fact]
        public async Task DecrementStockAsync_ShouldLogErrorOnNotFoundException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductService>>();

            // Create a context with no product having ID 1
            var productDbContext = CreateDatabaseWithTestProducts();

            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);
            var productService = new ProductService(
                productDbContext, CreateMapper(), mockLogger.Object, mockProductIdGenerator.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => productService.DecrementStockAsync(10, 1));
            Assert.Equal("Product with ID 10 not found.", exception.Message);
        }

        [Fact]
        public async Task DecrementStockAsync_ShouldLogErrorOnException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductService>>();
            var productDbContext = CreateDatabaseWithTestProducts();

            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);
            var productService = new ProductService(
                productDbContext, CreateMapper(), mockLogger.Object, mockProductIdGenerator.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DataAccessException>(() => productService.DecrementStockAsync(1, 1000));
            Assert.Equal("Not enough stock available.", exception.Message);
        }

        [Fact]
        public async Task AddToStockAsync_ShouldAddStock_WhenStockIsAdded()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product", StockAvailable = 15 };
            var productDbContext = CreateDatabaseWithTestProducts();
            var mockLogger = new Mock<ILogger<ProductService>>();

            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);
            var productService = new ProductService(
                productDbContext, CreateMapper(), mockLogger.Object, mockProductIdGenerator.Object);

            // Act
            await productService.AddToStockAsync(1, 10);

            // Assert
            Assert.Equal(15, product.StockAvailable);
        }

        [Fact]
        public async Task AddToStockAsync_ShouldLogErrorOnNotFoundException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductService>>();
            var productDbContext = CreateDatabaseWithTestProducts();

            var mockProductIdGenerator = new Mock<ProductIdGenerator>(1);
            var productService = new ProductService(
                productDbContext, CreateMapper(), mockLogger.Object, mockProductIdGenerator.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => productService.AddToStockAsync(10, 1));
            Assert.Equal("Product with ID 10 not found.", exception.Message);
        }

        private static IMapper CreateMapper()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile(typeof(ProductProfile))).CreateMapper();
            return mapper;
        }

        private static ProductDbContext CreateEmptyDatabase()
        {
            var context = new InMemoryDatabaseContext<ProductDbContext>().Context;
            return context;
        }

        private static ProductDbContext CreateDatabaseWithTestProducts()
        {
            var context = new InMemoryDatabaseContext<ProductDbContext>().Context;
            context.Product.Add(new Product
            {
                Id = 1,
                ProductId = 100000,
                Name = "Product 1",
                Description = "Description 1",
                Price = 10.0m,
                StockAvailable = 100,
            });
            context.SaveChanges();
            return context;
        }
    }
}
