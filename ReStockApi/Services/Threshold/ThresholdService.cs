using Microsoft.EntityFrameworkCore;
using ReStockApi.Models;
using ReStockApi.Services.Product;
using ReStockApi.Services.Store;

namespace ReStockApi.Services.Threshold
{
    public class ThresholdService : IThresholdService
    {
        private readonly ReStockDbContext _db;
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;
        private readonly ILogger<ThresholdService> _logger;

        public ThresholdService(ReStockDbContext db, IStoreService storeService, IProductService productService, ILogger<ThresholdService> logger)
        {
            _db = db;
            _storeService = storeService;
            _productService = productService;
            _logger = logger;
        }

        public async Task CreateThreshold(InventoryThreshold threshold)
        {
            await _db.InventoryThresholds.AddAsync(threshold);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteThresholdAsync(int storeNo, string ItemNo)
        {
            var threshold = _db.InventoryThresholds.FirstOrDefault(threshold => threshold.StoreNo == storeNo && threshold.ItemNo == ItemNo);
            if (threshold != null)
            {
                _db.InventoryThresholds.Remove(threshold);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<InventoryThreshold> GetThresholdAsync(int storeNo, string ItemNo)
            => await _db.InventoryThresholds.FirstOrDefaultAsync(threshold => threshold.StoreNo == storeNo && threshold.ItemNo == ItemNo);

        public async Task<IEnumerable<InventoryThreshold>> GetThresholdsAsync()
            => await _db.InventoryThresholds.ToListAsync();

        public async Task<IEnumerable<InventoryThreshold>> GetThresholdsByStoreNoAsync(int storeNo)
            => await _db.InventoryThresholds.Where(threshold => threshold.StoreNo == storeNo).ToListAsync();

        public async Task SyncThresholds()
        {
            var activeProducts = await _productService.GetProductsAsync();
            var stores = await _storeService.GetAllStores();

            foreach (var store in stores)
            {
                foreach (var product in activeProducts)
                {
                    var threshold = await GetThresholdAsync(store.No, product.ItemNo);
                    if (threshold != null && product.IsActive)
                        continue;

                    if (threshold != null && !product.IsActive)
                    {
                        await DeleteThresholdAsync(store.No, product.ItemNo);
                        continue;
                    }

                    if (threshold == null && product.IsActive)
                    {
                        threshold = new InventoryThreshold
                        {
                            StoreNo = store.No,
                            ItemNo = product.ItemNo,
                            MinimumQuantity = 0,
                            TargetQuantity = 0,
                            ReorderQuantity = 0,
                            LastUpdated = DateTime.UtcNow
                        };

                        await CreateThreshold(threshold);
                    }
                }
            }
        }

        public async Task UpdateThresholdAsync(InventoryThreshold threshold)
        {
            _db.InventoryThresholds.Update(threshold);
            await _db.SaveChangesAsync();
        }
    }
}
