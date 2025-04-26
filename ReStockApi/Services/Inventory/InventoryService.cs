using Microsoft.EntityFrameworkCore;
using ReStockApi.DTOs;
using ReStockApi.Models;
using System.Security.Cryptography;

namespace ReStockApi.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly ReStockDbContext _db;

        public InventoryService(ReStockDbContext db)
        {
            _db = db;
        }

        public async Task<DistributionCenterInventory> GetDistributionCenterInventoryAsync(string ItemNo)
            => await _db.DistributionCenterInventories.FirstOrDefaultAsync(x => x.ItemNo == ItemNo);

        public async Task<List<DistributionCenterInventory>> GetDistributionCenterInventoryAsync()
            => await _db.DistributionCenterInventories.ToListAsync();

        public async Task<StoreInventory> GetStoreInventoryAsync(int storeNo, string ItemNo)
            => await _db.StoreInventories.FirstOrDefaultAsync(x => x.StoreNo == storeNo && x.ItemNo == ItemNo);

        public Task<List<StoreInventory>> GetStoreInventoryByStoreNoAsync(int storeNo)
            => _db.StoreInventories.Where(x => x.StoreNo == storeNo).ToListAsync();

        public async Task<List<StoresInventoryWithThresholdDTO>> GetStoreInventoryByStoreNoWithThresholdsAsync(int storeNo)
        {
            var currentQty = await _db.StoreInventories
                .Where(x => x.StoreNo == storeNo)
                .Select(x => new StoresInventoryWithThresholdDTO
                {
                    StoreNo = x.StoreNo,
                    ItemNo = x.ItemNo,
                    CurrentQuantity = x.Quantity
                })
                .ToListAsync();

            var thresholds = await _db.InventoryThresholds
                .Where(x => x.StoreNo == storeNo)
                .Select(x => new StoresInventoryWithThresholdDTO
                {
                    StoreNo = x.StoreNo,
                    ItemNo = x.ItemNo,
                    MinimumQuantity = x.MinimumQuantity,
                    TargetQuantity = x.TargetQuantity,
                    ReorderQuantity = x.ReorderQuantity
                })
                .ToListAsync();

            return currentQty.Join(
                thresholds,
                current => new { current.StoreNo, current.ItemNo },
                threshold => new { threshold.StoreNo, threshold.ItemNo },
                (current, threshold) => new StoresInventoryWithThresholdDTO
                {
                    StoreNo = current.StoreNo,
                    ItemNo = current.ItemNo,
                    CurrentQuantity = current.CurrentQuantity,
                    MinimumQuantity = threshold.MinimumQuantity,
                    TargetQuantity = threshold.TargetQuantity,
                    ReorderQuantity = threshold.ReorderQuantity
                })
                .ToList();
        }

        public async Task UpsertDistributionCenterInventoryAsync(DistributionCenterInventory inventory)
        {
            var temp = await GetDistributionCenterInventoryAsync(inventory.ItemNo);
            if (temp == null)
                await _db.DistributionCenterInventories.AddAsync(inventory);
            else
            {
                temp.Quantity = inventory.Quantity;
                temp.LastUpdated = DateTime.UtcNow;
                _db.DistributionCenterInventories.Update(temp);
            }
        }

        public async Task UpsertStoreInventoryAsync(StoreInventory inventory)
        {
            var temp = await GetStoreInventoryAsync(inventory.StoreNo, inventory.ItemNo);

            if (temp == null)
                await _db.StoreInventories.AddAsync(inventory);
            else
            {
                temp.Quantity = inventory.Quantity;
                temp.LastUpdated = DateTime.UtcNow;
                _db.StoreInventories.Update(temp);
            }
        }
    }
}
