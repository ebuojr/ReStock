using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Initializes a new instance of the ReorderService class.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="inventoryService">The inventory service.</param>
        /// <param name="reorderLogService">The reorder log service.</param>
        /// <param name="storeService">The store service.</param>
        public ReorderService(ReStockDbContext db, IInventoryService inventoryService, IReorderLogService reorderLogService, IStoreService storeService)
        {
            _db = db;
            _InventoryService = inventoryService;
            _ReorderLogService = reorderLogService;
            _StoreService = storeService;
        }

        /// <summary>
        /// Creates potential reorder orders for a given store based on inventory thresholds and distribution center availability.
        /// </summary>
        /// <param name="storeNo">The store number.</param>
        /// <returns>A list of reorder orders to be created for the store.</returns>
        public async Task<List<Models.Reorder>> CreatePotentialOrdersByStoreNoAsync(int storeNo)
        {
            var result = new List<Models.Reorder>();

            await _StoreService.StoreExists(storeNo);

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
                    {
                        await _ReorderLogService
                            .LogAsync(
                            storeNo,
                            item.ItemNo,
                            0,
                            ReorderLogType.DCInventory.ToString(),
                            $"Distribution center inventory for item {item.ItemNo} is less than 1.", false);

                        continue;
                    }

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
                        CreatedAt = DateTime.UtcNow
                    });

                    // Log the reorder
                    await _ReorderLogService
                        .LogAsync(
                        storeNo,
                        item.ItemNo,
                        reorderAmount,
                        ReorderLogType.Reorder.ToString(),
                        $"Reordered {reorderAmount} of item {item.ItemNo} for store {storeNo}.", true);
                }
            }

            return result;
        }
    }
}
