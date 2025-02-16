// <copyright file="IProductService.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

using ProductInventoryAPI.Dtos;

namespace ProductInventoryAPI.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();

        Task<ProductDto> GetProductByIdAsync(int id);

        Task<ProductDto> AddProductAsync(ProductUpsertDto createDto);

        Task<ProductDto> UpdateProductAsync(int id, ProductUpsertDto updateDto);

        Task DeleteProductAsync(int id);

        Task DecrementStockAsync(int id, int quantity);

        Task AddToStockAsync(int id, int quantity);
    }
}
