using ReStockDomain;

namespace ReStockService.Inventory
{
    public interface IInventoryService
    {
        Task<StoreInventory> GetStoreInventoryAsync(int storeNo, string productNo);
        Task UpdateStoreInventoryAsync(StoreInventory inventory);
        Task<DistributionCenterInventory> GetDistributionCenterInventoryAsync(string productNo);
        Task UpdateDistributionCenterInventoryAsync(DistributionCenterInventory inventory);
    }
}
