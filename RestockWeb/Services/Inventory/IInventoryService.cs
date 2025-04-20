using RestockWeb.Models;

namespace RestockWeb.Services.Inventory
{
    public interface IInventoryService
    {
        Task<List<StoreInventory>?> GetStoreInventoryByStoreNoAsync(int storeNo);
        Task<StoreInventory?> GetStoreInventoryAsync(int storeNo, string itemNo);
        Task UpdateStoreInventoryAsync(StoreInventory inventory);
        Task<DistributionCenterInventory?> GetDistributionCenterInventoryAsync(string itemNo);
        Task<bool> UpdateDistributionCenterInventoryAsync(DistributionCenterInventory inventory);
    }
}