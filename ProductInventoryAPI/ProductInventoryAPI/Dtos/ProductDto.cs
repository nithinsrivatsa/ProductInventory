// <copyright file="ProductDto.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

namespace ProductInventoryAPI.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        public long ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int StockAvailable { get; set; }
    }
}
