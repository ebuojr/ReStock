using ReStockApi.DTOs;
using ReStockApi.Models;
using ReStockApi.Services.Inventory;
using ReStockApi.Services.ReorderLog;
using ReStockApi.Services.Store;

namespace ReStockApi.Services.Reorder
{
    public class ReorderService : IReorderService
    {
        private readonly ReStockDbContext _db;
        private readonly IInventoryService _InventoryService;
        private readonly IReorderLogService _ReorderLogService;
        private readonly IStoreService _StoreService;

        public ReorderService(ReStockDbContext db, IInventoryService inventoryService, IReorderLogService reorderLogService, IStoreService storeService)
        {
            _db = db;
            _InventoryService = inventoryService;
            _ReorderLogService = reorderLogService;
            _StoreService = storeService;
        }

        public async Task<List<Models.Reorder>> CreatePotentialOrdersByStoreNoAsync(int storeNo)
        {
            var result = new List<Models.Reorder>();

            var storeExists = await _StoreService.GetStore(storeNo);
            if (storeExists == null)
                throw new ArgumentNullException(nameof(storeNo), "Store number does not exist.");

            var thresholds = await _InventoryService.GetStoreInventoryByStoreNoWithThresholdsAsync(storeNo);

            if (thresholds == null || !thresholds.Any())
                throw new ArgumentNullException(nameof(thresholds), "No thresholds found for the given store number.");

            foreach (var item in thresholds)
            {
                if (item.CurrentQuantity <= item.MinimumQuantity)
                {
                    int reorderAmount = item.TargetQuantity - item.CurrentQuantity;

                    // check if the reorder amount
                    // is less than the minimum reorder quantity
                    if (reorderAmount < item.ReorderQuantity)
                    {
                        // Log the minimum ordering requirement
                        await _ReorderLogService
                            .LogAsync(
                            storeNo,
                            item.ItemNo,
                            reorderAmount,
                            ReorderLogType.MinimumReorder.ToString(),
                            $"The minimum ordering for item {item.ItemNo} in store {storeNo} is {item.ReorderQuantity}.", false);

                        continue;
                    }

                    // take the largest amount
                    reorderAmount = Math.Max(item.ReorderQuantity, item.TargetQuantity - item.CurrentQuantity);

                    // check dc inventory
                    var dcInventory = await _InventoryService.GetDistributionCenterInventoryAsync(item.ItemNo);

                    if (dcInventory.Quantity < 1)
                        throw new ArgumentNullException(nameof(dcInventory), "Distribution center inventory is less than 1.");

                    if (dcInventory.Quantity < reorderAmount)
                    {
                        // Log the distribution center inventory
                        await _ReorderLogService
                            .LogAsync(
                            storeNo,
                            item.ItemNo,
                            reorderAmount,
                            ReorderLogType.DCInventory.ToString(),
                            $"The distribution center inventory for item {item.ItemNo} is less than the reorder amount.", false);

                        continue;
                    }

                    result.Add(new Models.Reorder
                    {
                        StoreNo = storeNo,
                        ItemNo = item.ItemNo,
                        Quantity = reorderAmount,
                        CreatedAt = DateTime.UtcNow,
                    });
                }
            }

            return result;
        }

        public async Task ProcessReorderAsync(List<Models.Reorder> reorders)
        {
            List<Models.Reorder> reorderToInsert = new();

            var dcInventory = await _InventoryService.GetDistributionCenterInventoryAsync();

            foreach (var reorder in reorders)
            {
                var dcItem = dcInventory.FirstOrDefault(x => x.ItemNo == reorder.ItemNo);

                // check if the distribution center inventory
                // is less than the reorder amount
                if (dcItem.Quantity < reorder.Quantity)
                    continue;

                // update the distribution center inventory
                dcItem.Quantity -= reorder.Quantity;
                // update the store inventory
                var storeInventory = await _InventoryService.GetStoreInventoryAsync(reorder.StoreNo, reorder.ItemNo);

                storeInventory.Quantity += reorder.Quantity;

                // update the inventories
                await _InventoryService.UpsertDistributionCenterInventoryAsync(dcItem);
                await _InventoryService.UpsertStoreInventoryAsync(storeInventory);

                // add the reorder to the list
                reorderToInsert.Add(reorder);
            }

            await InsertReorderAsync(reorderToInsert);
        }

        public async Task InsertReorderAsync(List<Models.Reorder> reorders)
        {
            await _db.Reorders.AddRangeAsync(reorders);
            _db.SaveChanges();
        }
    }
}
