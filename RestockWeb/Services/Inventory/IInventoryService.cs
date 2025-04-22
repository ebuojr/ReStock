using RestockWeb.Models;

namespace RestockWeb.Services.Inventory
{
    public interface IInventoryService
    {
        Task<List<StoreInventory>?> GetStoreInventoryByStoreNoAsync(int storeNo);
        Task<StoreInventory?> GetStoreInventoryAsync(int storeNo, string itemNo);
        Task UpsertStoreInventoryAsync(StoreInventory inventory);
        Task<List<DistributionCenterInventory>> GetDistributionCenterInventories();
        Task<DistributionCenterInventory?> GetDistributionCenterInventoryByItemNo(string itemNo);
        Task<bool> UpsertDistributionCenterInventoryAsync(DistributionCenterInventory inventory);
    }
}