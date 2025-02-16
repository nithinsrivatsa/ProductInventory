// <copyright file="ProductUpsertDto.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

namespace ProductInventoryAPI.Dtos
{
    public class ProductUpsertDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int StockAvailable { get; set; }
    }
}
