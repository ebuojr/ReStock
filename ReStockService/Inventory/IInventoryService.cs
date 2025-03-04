using ReStockDomain;

namespace ReStockService.Inventory
{
    public interface IInventoryService
    {
        Task<StoreInventory> GetStoreInventoryAsync(int storeNo, string ItemNo);
        Task UpdateStoreInventoryAsync(StoreInventory inventory);
        Task<DistributionCenterInventory> GetDistributionCenterInventoryAsync(string ItemNo);
        Task UpdateDistributionCenterInventoryAsync(DistributionCenterInventory inventory);
    }
}
