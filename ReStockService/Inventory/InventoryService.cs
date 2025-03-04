using Microsoft.EntityFrameworkCore;
using ReStockDomain;

namespace ReStockService.Inventory
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

        public async Task<StoreInventory> GetStoreInventoryAsync(int storeNo, string ItemNo)
            => await _db.StoreInventories.FirstOrDefaultAsync(x => x.StoreNo == storeNo && x.ItemNo == ItemNo);

        public async Task UpdateDistributionCenterInventoryAsync(DistributionCenterInventory inventory)
        {
            _db.DistributionCenterInventories.Update(inventory);
            await _db.SaveChangesAsync();
        }

        public Task UpdateStoreInventoryAsync(StoreInventory inventory)
        {
            _db.StoreInventories.Update(inventory);
            return _db.SaveChangesAsync();
        }
    }
}
