using ReStockApi.Models;

namespace ReStockApi.Services.Inventory
{
    public interface IInventoryService
    {
        Task<List<StoreInventory>> GetStoreInventoryByStoreNoAsync(int storeNo);
        Task<StoreInventory> GetStoreInventoryAsync(int storeNo, string ItemNo);
        Task UpsertStoreInventoryAsync(StoreInventory inventory);
        Task<List<DistributionCenterInventory>> GetDistributionCenterInventoryAsync();
        Task<DistributionCenterInventory> GetDistributionCenterInventoryAsync(string ItemNo);
        Task UpsertDistributionCenterInventoryAsync(DistributionCenterInventory inventory);
    }
}
