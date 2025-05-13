using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReStockApi;
using ReStockApi.Models;

namespace ReStockApiTest.IntegrationTest
{
    // Custom factory to use InMemory DB for integration tests
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("IntegrationTest");

            builder.ConfigureServices(services =>
            {
                // Remove the app's DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ReStockDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                // Add a new DbContext using InMemory
                services.AddDbContext<ReStockDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Remove background services for integration tests
                var hostedServiceDescriptors = services.Where(d => d.ServiceType.Name.Contains("HostedService")).ToList();
                foreach (var hostedService in hostedServiceDescriptors)
                {
                    services.Remove(hostedService);
                }

                // Build the service provider
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database context
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ReStockDbContext>();
                    db.Database.EnsureCreated();

                    // Clear existing data to avoid duplicate key errors
                    db.Stores.RemoveRange(db.Stores);
                    db.Products.RemoveRange(db.Products);
                    db.InventoryThresholds.RemoveRange(db.InventoryThresholds);
                    db.StoreInventories.RemoveRange(db.StoreInventories);
                    db.DistributionCenterInventories.RemoveRange(db.DistributionCenterInventories);
                    db.JobLastRuns.RemoveRange(db.JobLastRuns);
                    db.SalesOrderNumber.RemoveRange(db.SalesOrderNumber);
                    db.SaveChanges();

                    // Seed minimal required data for integration tests with realistic data patterns
                    db.Stores.Add(new Store { No = 5001, Name = "Test Store", Country = "Test Country", Address = "Test Address" });
                    db.Stores.Add(new Store { No = 5090, Name = "Second Test Store", Country = "Test Country 2", Address = "Test Address 2" });
                    
                    db.Products.Add(new Product { ItemNo = "ZIZ-111-1111", Name = "Test Product", Brand = "BrandX", RetailPrice = 10.0m, IsActive = true });
                    
                    db.InventoryThresholds.Add(new InventoryThreshold { StoreNo = 5001, ItemNo = "ZIZ-111-1111", MinimumQuantity = 3, TargetQuantity = 15, ReorderQuantity = 8, LastUpdated = DateTime.UtcNow });
                    db.InventoryThresholds.Add(new InventoryThreshold { StoreNo = 5090, ItemNo = "ZIZ-111-1111", MinimumQuantity = 3, TargetQuantity = 15, ReorderQuantity = 8, LastUpdated = DateTime.UtcNow });
                    
                    db.StoreInventories.Add(new StoreInventory { StoreNo = 5001, ItemNo = "ZIZ-111-1111", Quantity = 10, LastUpdated = DateTime.UtcNow });
                    db.StoreInventories.Add(new StoreInventory { StoreNo = 5090, ItemNo = "ZIZ-111-1111", Quantity = 2, LastUpdated = DateTime.UtcNow }); // Low quantity to trigger reorder
                    
                    db.DistributionCenterInventories.Add(new DistributionCenterInventory { ItemNo = "ZIZ-111-1111", Quantity = 100, LastUpdated = DateTime.UtcNow });
                    
                    db.JobLastRuns.Add(new JobLastRun { Type = "ThresholdProductSync", LastRunTime = DateTime.UtcNow.AddDays(-1) });
                    
                    // Seed SalesOrderNumber for integration tests
                    db.SalesOrderNumber.Add(new SalesOrderNumber { No = 1 });
                    
                    db.SaveChanges();
                }
            });
        }
    }
}
