using Microsoft.EntityFrameworkCore;
using ReStockApi.Models;

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
