// <copyright file="ProductIdGeneratorTests.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

namespace ProductInventoryAPI.Tests.ServicesTests
{
    using ProductInventoryAPI.Helper;
    using Xunit;

    public class ProductIdGeneratorTests
    {
        private readonly ProductIdGenerator productIdGenerator;

        public ProductIdGeneratorTests()
        {
            productIdGenerator = new ProductIdGenerator(1); // Use Node ID = 1 for testing
        }

        [Fact]
        public void GenerateProductId_ShouldReturnSixDigitId()
        {
            long productId = productIdGenerator.GenerateProductId();
            Assert.InRange(productId, 100000, 999999);
        }

        [Fact]
        public void Constructor_ShouldThrowException_ForInvalidNodeId()
        {
            Assert.Throws<ArgumentException>(() => new ProductIdGenerator(-1));
            Assert.Throws<ArgumentException>(() => new ProductIdGenerator(10));
        }

        [Fact]
        public void GenerateProductId_ShouldThrowException_IfClockMovesBackward()
        {
            var generator = new ProductIdGenerator(1);

            // Simulate clock rollback
            typeof(ProductIdGenerator)
                .GetField("lastTimestamp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 10000);
            Assert.Throws<Exception>(() => generator.GenerateProductId());
        }
    }
}
