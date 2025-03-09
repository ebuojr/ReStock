using ReStockApi.Models;

namespace ReStockApi.Services.Inventory
{
    public interface IInventoryService
    {
        Task<List<StoreInventory>> GetStoreInventoryByStoreNoAsync(int storeNo);
        Task<StoreInventory> GetStoreInventoryAsync(int storeNo, string ItemNo);
        Task UpdateStoreInventoryAsync(StoreInventory inventory);
        Task<DistributionCenterInventory> GetDistributionCenterInventoryAsync(string ItemNo);
        Task UpdateDistributionCenterInventoryAsync(DistributionCenterInventory inventory);
    }
}
