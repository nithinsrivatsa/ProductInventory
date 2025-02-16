// <copyright file="ProductDbContext.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

namespace ProductInventoryAPI.Repositories
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.EntityFrameworkCore;

    public class ProductDbContext : DbContext
    {
        [ExcludeFromCodeCoverage]
        public ProductDbContext(DbContextOptions<ProductDbContext> options, IConfiguration configuration)
        : base(options)
        {
            Configuration = configuration;
        }

        public ProductDbContext()
        {
        }

        public IConfiguration Configuration { get; }

        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.HasIndex(entity => entity.Id).IsUnique();
                entity.Property(entity => entity.ProductId)
                    .IsRequired();
            });
        }
    }
}
