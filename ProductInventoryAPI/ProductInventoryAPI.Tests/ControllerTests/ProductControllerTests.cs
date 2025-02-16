// <copyright file="ProductControllerTests.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

namespace ProductInventoryAPI.Tests.ControllerTests
{
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using ProductInventoryAPI.Controllers;
    using ProductInventoryAPI.Dtos;
    using ProductInventoryAPI.Interfaces;
    using Xunit;

    public class ProductControllerTests
    {
        private readonly Mock<IProductService> productServiceMock;
        private readonly ProductController controller;

        public ProductControllerTests()
        {
            productServiceMock = new Mock<IProductService>();
            controller = new ProductController(productServiceMock.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync()
        {
            // Arrange
            productServiceMock.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(CreateTestProductDtos());

            // Act
            var actionResult = await controller.GetAllProductsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var result = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetProductByIdAsync()
        {
            // Arrange
            productServiceMock.Setup(x => x.GetProductByIdAsync(1)).ReturnsAsync(CreateTestProductDto());

            // Act
            var actionResult = await controller.GetProductByIdAsync(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsAssignableFrom<ProductDto>(okResult.Value);
        }

        [Fact]
        public async Task AddProductAsync_ShouldReturnOk()
        {
            // Arrange
            productServiceMock.Setup(x => x.AddProductAsync(It.IsAny<ProductUpsertDto>())).Returns(Task.FromResult<ProductDto>(null));

            var createDto = new ProductUpsertDto
            {
                // Initialize properties as needed
                Name = "New Product",
                Description = "New Product Description",
                Price = 99.99m,
                StockAvailable = 50,
            };

            // Act
            var result = await controller.AddProductAsync(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Product added successfully.", okResult.Value);
            productServiceMock.Verify(x => x.AddProductAsync(It.IsAny<ProductUpsertDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldReturnOk()
        {
            // Arrange
            var updatedProductDto = new ProductDto
            {
                Id = 1,
                ProductId = 100000,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 20.0m,
                StockAvailable = 150,
            };

            productServiceMock.Setup(x => x.UpdateProductAsync(1, It.IsAny<ProductUpsertDto>())).ReturnsAsync(updatedProductDto);
            var updateDto = CreateTestProductUpsertDto();

            // Act
            var result = await controller.UpdateProductAsync(1, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Product updated successfully.", okResult.Value);
            productServiceMock.Verify(x => x.UpdateProductAsync(1, It.IsAny<ProductUpsertDto>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldReturnOk()
        {
            // Arrange
            productServiceMock.Setup(x => x.DeleteProductAsync(1));

            // Act
            var result = await controller.DeleteProductAsync(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Product deleted successfully.", okResult.Value);
            productServiceMock.Verify(x => x.DeleteProductAsync(1));
        }

        [Fact]
        public async Task AddToStockAsync_ShouldReturnOk()
        {
            // Arrange
            productServiceMock.Setup(x => x.AddToStockAsync(1, 10));

            // Act
            var result = await controller.AddToStockAsync(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Stock added successfully.", okResult.Value);
            productServiceMock.Verify(x => x.AddToStockAsync(1, 10));
        }

        [Fact]
        public async Task DecrementStockAsync_ShouldReturnOk()
        {
            // Arrange
            productServiceMock.Setup(x => x.DecrementStockAsync(1, 10));

            // Act
            var result = await controller.DecrementStockAsync(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Stock decremented successfully.", okResult.Value);
            productServiceMock.Verify(x => x.DecrementStockAsync(1, 10));
        }

        private static ProductUpsertDto CreateTestProductUpsertDto()
        {
            return new ProductUpsertDto
            {
                Name = "Product 1",
                Description = "Description 1",
                Price = 10.0m,
                StockAvailable = 100,
            };
        }

        private static ProductDto[] CreateTestProductDtos()
        {
            return
            [
                new ProductDto { Id = 1, ProductId = 100000, Name = "Product 1", Description = "Description 1", Price = 10.0m, StockAvailable = 100 },
                new ProductDto { Id = 2, ProductId = 100001, Name = "Product 2", Description = "Description 2", Price = 20.0m, StockAvailable = 200 },
            ];
        }

        private static ProductDto CreateTestProductDto()
        {
            return new ProductDto
            {
                Id = 1,
                ProductId = 100000,
                Name = "Product 1",
                Description = "Description 1",
                Price = 10.0m,
                StockAvailable = 100,
            };
        }
    }
}
