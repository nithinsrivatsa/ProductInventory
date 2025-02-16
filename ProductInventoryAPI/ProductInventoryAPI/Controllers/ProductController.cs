// <copyright file="ProductController.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

namespace ProductInventoryAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ProductInventoryAPI.Dtos;
    using ProductInventoryAPI.Interfaces;

    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            var products = await productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductByIdAsync(int id)
        {
            var products = await productService.GetProductByIdAsync(id);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductAsync(ProductUpsertDto createDto)
        {
            await productService.AddProductAsync(createDto);
            return Ok("Product added successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, [FromBody] ProductUpsertDto updateDto)
        {
            await productService.UpdateProductAsync(id, updateDto);
            return Ok("Product updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            await productService.DeleteProductAsync(id);
            return Ok("Product deleted successfully.");
        }

        [HttpPut("add-to-stock/{id}/{quantity}")]
        public async Task<IActionResult> AddToStockAsync(int id, int quantity)
        {
            await productService.AddToStockAsync(id, quantity);
            return Ok("Stock added successfully.");
        }

        [HttpPut("decrement-stock/{id}/{quantity}")]
        public async Task<IActionResult> DecrementStockAsync(int id, int quantity)
        {
            await productService.DecrementStockAsync(id, quantity);
            return Ok("Stock decremented successfully.");
        }
    }
}
