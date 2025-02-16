// <copyright file="Program.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

namespace ProductInventoryAPI
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.EntityFrameworkCore;
    using ProductInventoryAPI.Repositories;

    [ExcludeFromCodeCoverage]
    public class Program
    {
        private Program()
        {
        }

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            MigrateDatabase(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void MigrateDatabase(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Starting Product database schema migration if needed");
            try
            {
                var context = scope.ServiceProvider.GetService<ProductDbContext>();
                if (context == null)
                {
                    logger.LogError("ProductDbContext is null. Database migration cannot proceed.");
                    return;
                }

                context.Database.Migrate();
                logger.LogInformation("Product Database schema migration done");
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Error on Product database schema migration");
            }
        }
    }
}
