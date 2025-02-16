// <copyright file="ProductService.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

namespace ProductInventoryAPI.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using ProductInventoryAPI.Dtos;
    using ProductInventoryAPI.Helper;
    using ProductInventoryAPI.Interfaces;
    using ProductInventoryAPI.Repositories;
    using ProductInventoryAPI.Services.Exceptions;

    public class ProductService : IProductService
    {
        private readonly ProductDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<ProductService> logger;
        private readonly ProductIdGenerator productIdGenerator;

        public ProductService(ProductDbContext context, IMapper mapper, ILogger<ProductService> logger, ProductIdGenerator productIdGenerator)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
            this.productIdGenerator = productIdGenerator;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                var productsData = await context.Product
                   .Select(p => new ProductDto
                   {
                       Id = p.Id,
                       ProductId = p.ProductId,
                       Name = p.Name,
                       Description = p.Description,
                       Price = p.Price,
                       StockAvailable = p.StockAvailable,
                   })
                   .ToListAsync();
                var result = mapper.Map<IEnumerable<ProductDto>>(productsData);
                if (!result.Any())
                {
                    logger.LogWarning("Products data are not available in the database");
                    return new List<ProductDto>();
                }

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error while fetching the products data from the database: {ex.Message}");
                throw new DataAccessException("Failed to fetch product data.", ex);
            }
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            try
            {
                var productEntity = await context.Product.AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);
                if (productEntity == null)
                {
                    logger.LogWarning($"Product data not available for the selected Id: {id}");
                    throw new NotFoundException($"Product with ID {id} not found.");
                }

                return mapper.Map<ProductDto>(productEntity);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error while fetching the product by Id: {id} from the database: {ex.Message}");
                throw new DataAccessException("Failed to fetch product data.", ex);
            }
        }

        public async Task<ProductDto> AddProductAsync(ProductUpsertDto createDto)
        {
            try
            {
                var productEntity = new Product
                {
                    ProductId = productIdGenerator.GenerateProductId(),
                    Name = createDto.Name,
                    Description = createDto.Description,
                    Price = createDto.Price,
                    StockAvailable = createDto.StockAvailable,
                };
                var addedProduct = await context.Product.AddAsync(productEntity);
                await context.SaveChangesAsync();

                return new ProductDto
                {
                    ProductId = addedProduct.Entity.ProductId,
                    Name = addedProduct.Entity.Name,
                    StockAvailable = addedProduct.Entity.StockAvailable,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding a product.");
                throw new DataAccessException("Failed to add product.", ex);
            }
        }

        public async Task<ProductDto> UpdateProductAsync(int id, ProductUpsertDto updateDto)
        {
            try
            {
                var existingProduct = await context.Product.FindAsync(id) ?? throw new NotFoundException($"Product with ID {id} not found.");

                // Update properties
                mapper.Map(updateDto, existingProduct);
                context.Product.Update(existingProduct);
                await context.SaveChangesAsync();
                return mapper.Map<ProductDto>(existingProduct);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error updating product with ID {id}.");
                throw new DataAccessException("Failed to update product.", ex);
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var existingProduct = await context.Product.FindAsync(id) ?? throw new NotFoundException($"Product with ID {id} not found.");
                context.Product.Remove(existingProduct);
                await context.SaveChangesAsync();
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error deleting product with ID {id}.");
                throw new DataAccessException("Failed to delete product.", ex);
            }
        }

        public async Task DecrementStockAsync(int id, int quantity)
        {
            var product = await context.Product.FindAsync(id) ?? throw new NotFoundException($"Product with ID {id} not found.");
            if (product.StockAvailable < quantity)
            {
                throw new DataAccessException("Not enough stock available.", null);
            }

            product.StockAvailable -= quantity;
            await context.SaveChangesAsync();
        }

        public async Task AddToStockAsync(int id, int quantity)
        {
            var product = await context.Product.FindAsync(id) ?? throw new NotFoundException($"Product with ID {id} not found.");
            product.StockAvailable += quantity;
            await context.SaveChangesAsync();
        }
    }
}
