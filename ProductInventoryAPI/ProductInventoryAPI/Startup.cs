// <copyright file="Startup.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

namespace ProductInventoryAPI
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.OpenApi.Models;
    using ProductInventoryAPI.Helper;
    using ProductInventoryAPI.Interfaces;
    using ProductInventoryAPI.Repositories;
    using ProductInventoryAPI.Services;

    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddRazorPages();
            services.AddDbContext<ProductDbContext>(options =>
                                    options.UseSqlServer(
                                        Configuration.GetConnectionString("ProductInventoryConnectionToSqlServer"),
                                        opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds)));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Product Inventory API",
                        Version = "v1",
                    });
            });
            services.AddMemoryCache();

            // Dependency Injection for the services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IProductService, ProductService>();
            services.AddSingleton(new ProductIdGenerator(nodeId: 1));

            services.AddLogging(loggerBuilder =>
            {
                loggerBuilder.ClearProviders();
                loggerBuilder.AddLog4Net("ProductInventory.config", true);
            });
        }
    }
}
