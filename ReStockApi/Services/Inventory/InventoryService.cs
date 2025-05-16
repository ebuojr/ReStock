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

        /// <summary>
        /// Checks the availability of a given item in the distribution center inventory.
        /// </summary>
        /// <param name="itemNo">The item number to check.</param>
        /// <param name="quantity">The requested quantity.</param>
        /// <returns>The available quantity (may be less than requested).</returns>
        public async Task<int> CheckAvailabilityAsync(string itemNo, int quantity)
        {
            var inv = await _db.DistributionCenterInventories.FirstOrDefaultAsync(x => x.ItemNo == itemNo);
            if (inv == null)
                throw new Exception("Inventory not found");
            if (inv.Quantity < quantity)
                return inv.Quantity;
            return quantity;
        }

        /// <summary>
        /// Decreases the inventory quantity for a specific item in a store.
        /// </summary>
        /// <param name="storeNo">The store number.</param>
        /// <param name="itemNo">The item number.</param>
        /// <param name="quantity">The quantity to decrease.</param>
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

        /// <summary>
        /// Decreases the inventory quantity for a specific item in the distribution center.
        /// </summary>
        /// <param name="itemNo">The item number.</param>
        /// <param name="quantity">The quantity to decrease.</param>
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

        /// <summary>
        /// Gets the distribution center inventory for a specific item.
        /// </summary>
        /// <param name="ItemNo">The item number.</param>
        /// <returns>The distribution center inventory record.</returns>
        public async Task<DistributionCenterInventory> GetDistributionCenterInventoryAsync(string ItemNo)
            => await _db.DistributionCenterInventories.FirstOrDefaultAsync(x => x.ItemNo == ItemNo);

        /// <summary>
        /// Gets all distribution center inventory records.
        /// </summary>
        /// <returns>A list of all distribution center inventory records.</returns>
        public async Task<List<DistributionCenterInventory>> GetDistributionCenterInventoryAsync()
            => await _db.DistributionCenterInventories.ToListAsync();

        /// <summary>
        /// Gets the store inventory for a specific store and item.
        /// </summary>
        /// <param name="storeNo">The store number.</param>
        /// <param name="ItemNo">The item number.</param>
        /// <returns>The store inventory record.</returns>
        public async Task<StoreInventory> GetStoreInventoryAsync(int storeNo, string ItemNo)
            => await _db.StoreInventories.FirstOrDefaultAsync(x => x.StoreNo == storeNo && x.ItemNo == ItemNo);

        /// <summary>
        /// Gets all store inventory records for a specific store.
        /// </summary>
        /// <param name="storeNo">The store number.</param>
        /// <returns>A list of store inventory records.</returns>
        public Task<List<StoreInventory>> GetStoreInventoryByStoreNoAsync(int storeNo)
            => _db.StoreInventories.Where(x => x.StoreNo == storeNo).ToListAsync();

        /// <summary>
        /// Gets all store inventory records with thresholds for a specific store.
        /// </summary>
        /// <param name="storeNo">The store number.</param>
        /// <returns>A list of inventory records with threshold information.</returns>
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

        /// <summary>
        /// Increases the inventory quantity for a specific item in a store.
        /// </summary>
        /// <param name="storeNo">The store number.</param>
        /// <param name="itemNo">The item number.</param>
        /// <param name="quantity">The quantity to increase.</param>
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

        /// <summary>
        /// Inserts or updates a distribution center inventory record.
        /// </summary>
        /// <param name="inventory">The inventory record to upsert.</param>
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

        /// <summary>
        /// Inserts or updates a store inventory record after validation.
        /// </summary>
        /// <param name="inventory">The inventory record to upsert.</param>
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
