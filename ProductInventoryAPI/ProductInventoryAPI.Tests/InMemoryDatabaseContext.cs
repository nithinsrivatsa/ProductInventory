// <copyright file="InMemoryDatabaseContext.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

namespace ProductInventoryAPI.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.Configuration;

#nullable disable

    public class InMemoryDatabaseContext<T> : IDisposable
        where T : DbContext
    {
        public InMemoryDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<T>()
              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString(), b => b.EnableNullChecks(false))
              .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
              .Options;
            Context = (T)Activator.CreateInstance(typeof(T), options, GetConfiguration());

            Context.Database.EnsureCreated();
        }

        public T Context { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }

        private static IConfiguration GetConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string> { { "Product_DB", "Product.db" } };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return configuration;
        }
    }
}
