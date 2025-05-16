using Bogus;
using Microsoft.EntityFrameworkCore;
using ReStockApi.Models;
using ReStockApi.Services.Inventory;
using ReStockApi.Services.Product;
using ReStockApi.Services.Store;
using ReStockApi.Services.Threshold;

namespace ReStockApi.Services.DataGeneration
{
    public class DataGenerationService : IDataGenerationService
    {
        private readonly IProductService _ProductService;
        private readonly IInventoryService _InventoryService;
        private readonly IThresholdService _ThresholdService;
        private readonly IStoreService _StoreService;
        private readonly ReStockDbContext _db;

        /// <summary>
        /// Initializes a new instance of the DataGenerationService class.
        /// </summary>
        /// <param name="productService">The product service.</param>
        /// <param name="inventoryService">The inventory service.</param>
        /// <param name="thresholdService">The threshold service.</param>
        /// <param name="storeService">The store service.</param>
        /// <param name="db">The database context.</param>
        public DataGenerationService(
            IProductService productService,
            IInventoryService inventoryService,
            IThresholdService thresholdService,
            IStoreService storeService,
            ReStockDbContext db)
        {
            _ProductService = productService;
            _InventoryService = inventoryService;
            _ThresholdService = thresholdService;
            _StoreService = storeService;
            _db = db;
        }

        /// <summary>
        /// Generates random inventory for the distribution center for all products.
        /// </summary>
        public async Task GenerateDCInventory()
        {
            var products = await _ProductService.GetProductsAsync();

            foreach (var product in products)
                await _InventoryService.UpsertDistributionCenterInventoryAsync(new Models.DistributionCenterInventory
                {
                    ItemNo = product.ItemNo,
                    Quantity = new Random().Next(50, 100),
                    LastUpdated = DateTime.Now
                });
        }

        /// <summary>
        /// Generates random product items and adds them to the database.
        /// </summary>
        public async Task GenerateProductItems()
        {
            string brand = "Zizzi";

            var clothingTypes = new[] { "Dress", "Shirt", "Trousers", "Jacket", "Skirt", "Blouse", "Sweater", "Coat", "Jeans", "T-Shirt" };
            var clothingAdjectives = new[] { "Casual", "Elegant", "Modern", "Vintage", "Chic", "Trendy", "Classic", "Bohemian" };
            var materials = new[] { "Cotton", "Silk", "Linen", "Wool", "Denim", "Leather", "Polyester" };

            var productFaker = new Faker<Models.Product>()
                .RuleFor(p => p.Name, f => $"{f.PickRandom(clothingAdjectives)} {f.PickRandom(clothingTypes)} made with {f.PickRandom(materials)}")
                .RuleFor(p => p.Brand, f => brand)
                .RuleFor(p => p.RetailPrice, f => f.Finance.Amount(79, 249))
                .RuleFor(p => p.ItemNo, f => $"ZIZ-{f.Random.Replace("###-####")}")
                .RuleFor(p => p.IsActive, f => f.PickRandom(new[] { true, false }));

            var products = productFaker.Generate(50);

            foreach (var product in products)
                await _ProductService.CreateProductAsync(product);
        }

        /// <summary>
        /// Generates random store inventory for all stores and products.
        /// </summary>
        public async Task GenerateStoreinventory()
        {
            var products = await _ProductService.GetProductsAsync();
            var stores = await _StoreService.GetAllStores();

            foreach (var store in stores)
                foreach (var product in products)
                    await _InventoryService.UpsertStoreInventoryAsync(new Models.StoreInventory
                    {
                        StoreNo = store.No,
                        ItemNo = product.ItemNo,
                        Quantity = new Random().Next(2, 15),
                        LastUpdated = DateTime.Now
                    });

        }

        /// <summary>
        /// Generates random inventory thresholds for all stores and products.
        /// </summary>
        public async Task GenereateInventoryThresholds()
        {
            var products = await _ProductService.GetProductsAsync();
            var stores = await _StoreService.GetAllStores();
            foreach (var store in stores)
                foreach (var product in products)
                    await _ThresholdService.CreateThreshold(new InventoryThreshold
                    {
                        StoreNo = store.No,
                        ItemNo = product.ItemNo,
                        MinimumQuantity = new Random().Next(2, 5),
                        TargetQuantity = new Random().Next(10, 20),
                        ReorderQuantity = new Random().Next(5, 10),
                        LastUpdated = DateTime.Now
                    });
        }

        /// <summary>
        /// Truncates (deletes all rows from) a table by its name and resets its auto-increment sequence.
        /// </summary>
        /// <param name="tableName">The name of the table to truncate.</param>
        public async Task TruncateTableByTableName(string tableName)
        {
            try
            {
                await _db.Database.ExecuteSqlRawAsync($"DELETE FROM {tableName}");
                await _db.Database.ExecuteSqlRawAsync($"DELETE FROM sqlite_sequence WHERE name = '{tableName}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error truncating table {tableName}: {ex.Message}");
                throw;
            }
        }
    }
}
