using ReStockApi.Models;
using ReStockApi.Services.Inventory;
using ReStockApi.Services.ReorderLog;
using ReStockApi.Services.Store;
using System.Security.Cryptography;

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
                        Console.WriteLine($"The minimum ordering for item {item.ItemNo} in store {storeNo} is {item.ReorderQuantity}.");
                        // Log the minimum ordering requirement
                        //await _ReorderLogService
                        //    .LogAsync(
                        //    storeNo,
                        //    item.ItemNo,
                        //    reorderAmount,
                        //    "Minimum Reorder",
                        //    $"The minimum ordering for item {item.ItemNo} in store {storeNo} is {item.ReorderQuantity}.", false);

                        reorderAmount = item.ReorderQuantity;
                        continue;
                    }

                    // take the largest amount
                    reorderAmount = Math.Max(item.ReorderQuantity, item.TargetQuantity - item.CurrentQuantity);

                    // check dc inventory
                    var dcInventory = await _InventoryService.GetDistributionCenterInventoryAsync(item.ItemNo);

                    if (reorderAmount > dcInventory.Quantity)
                    {
                        Console.WriteLine($"The reorder amount for item {item.ItemNo} in store {storeNo} is greater than the DC inventory.");
                        // Log the DC inventory check
                        //await _ReorderLogService
                        //    .LogAsync(
                        //    storeNo,
                        //    item.ItemNo,
                        //    reorderAmount,
                        //    "DC Inventory",
                        //    $"The reorder amount for item {item.ItemNo} in store {storeNo} is greater than the DC inventory.", false);
                        reorderAmount = dcInventory.Quantity;
                    }

                    result.Add(new Models.Reorder
                    {
                        StoreNo = storeNo,
                        ItemNo = item.ItemNo,
                        Quantity = reorderAmount
                    });
                }
            }

            return result;

            //await _db.Reorders.AddRangeAsync(result);
            //await _db.SaveChangesAsync();
        }


        public async Task<bool> ProcessReorderAsync(Models.Reorder reorder)
        {
            await _db.Reorders.AddAsync(reorder);
            _db.SaveChanges();
            return true;
        }
    }
}
