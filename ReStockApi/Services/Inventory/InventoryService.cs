using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ReStockApi.DTOs;
using ReStockApi.Models;

namespace ReStockApi.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly ReStockDbContext _db;
        private readonly IValidator<Models.StoreInventory> _validator;

        public InventoryService(ReStockDbContext db, IValidator<StoreInventory> validator)
        {
            _db = db;
            _validator = validator;
        }

        public async Task<int> CheckAvailabilityAsync(string itemNo, int quantity)
        {
            var inv = await _db.DistributionCenterInventories.FirstOrDefaultAsync(x => x.ItemNo == itemNo);
            if (inv == null)
                throw new Exception("Inventory not found");
            if (inv.Quantity < quantity)
                return inv.Quantity;
            return quantity;
        }

        public async Task DecreaseStoreInventoryAsync(int storeNo, string itemNo, int quantity)
        {
            var inv = await _db.StoreInventories.FirstOrDefaultAsync(x => x.StoreNo == storeNo && x.ItemNo == itemNo);
            if (inv == null)
                throw new Exception("Inventory not found");
            inv.Quantity -= quantity;
            if (inv.Quantity < 0)
                inv.Quantity = 0;
            inv.LastUpdated = DateTime.UtcNow;
            _db.StoreInventories.Update(inv);
            await _db.SaveChangesAsync();
        }

        public async Task DescreaseDistributionCenterInventoryAsync(string itemNo, int quantity)
        {
            var inv = await _db.DistributionCenterInventories.FirstOrDefaultAsync(x => x.ItemNo == itemNo);
            if (inv == null)
                throw new Exception("Inventory not found");
            inv.Quantity -= quantity;
            if (inv.Quantity < 0)
                inv.Quantity = 0;
            inv.LastUpdated = DateTime.UtcNow;
            _db.DistributionCenterInventories.Update(inv);
            await _db.SaveChangesAsync();
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

        public Task IncreaseStoreInventoryAsync(int storeNo, string itemNo, int quantity)
        {
            var inv = _db.StoreInventories.FirstOrDefault(x => x.StoreNo == storeNo && x.ItemNo == itemNo);
            if (inv == null)
                throw new Exception("Inventory not found");
            inv.Quantity += quantity;
            inv.LastUpdated = DateTime.UtcNow;
            _db.StoreInventories.Update(inv);
            return _db.SaveChangesAsync();
        }

        public async Task UpsertDistributionCenterInventoryAsync(DistributionCenterInventory inventory)
        {
            var temp = await GetDistributionCenterInventoryAsync(inventory.ItemNo);

            if (temp == null)
            {
                inventory.LastUpdated = DateTime.UtcNow;
                await _db.DistributionCenterInventories.AddAsync(inventory);
            }
            else
            {
                // Update existing inventory record
                temp.Quantity = inventory.Quantity;
                temp.LastUpdated = DateTime.UtcNow;
                _db.DistributionCenterInventories.Update(temp);
            }

            await _db.SaveChangesAsync();
        }

        public async Task UpsertStoreInventoryAsync(StoreInventory inventory)
        {
            // validate
            var result = await _validator.ValidateAsync(inventory);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var temp = await GetStoreInventoryAsync(inventory.StoreNo, inventory.ItemNo);

            if (temp == null)
            {
                inventory.LastUpdated = DateTime.UtcNow;
                await _db.StoreInventories.AddAsync(inventory);
            }
            else
            {
                temp.Quantity = inventory.Quantity;
                temp.LastUpdated = DateTime.UtcNow;
                _db.StoreInventories.Update(temp);
            }

            await _db.SaveChangesAsync();
        }
    }
}
