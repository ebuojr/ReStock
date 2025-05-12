using ReStockApi.DTOs;
using ReStockApi.Models;
using System.Security.Cryptography;

namespace ReStockApi.Services.Inventory
{
    public interface IInventoryService
    {
        Task<List<StoresInventoryWithThresholdDTO>> GetStoreInventoryByStoreNoWithThresholdsAsync(int storeNo);
        Task<List<StoreInventory>> GetStoreInventoryByStoreNoAsync(int storeNo);
        Task<StoreInventory> GetStoreInventoryAsync(int storeNo, string ItemNo);
        Task UpsertStoreInventoryAsync(StoreInventory inventory);
        Task<List<DistributionCenterInventory>> GetDistributionCenterInventoryAsync();
        Task<DistributionCenterInventory> GetDistributionCenterInventoryAsync(string ItemNo);
        Task UpsertDistributionCenterInventoryAsync(DistributionCenterInventory inventory);
        Task<int> CheckAvailabilityAsync(string itemNo, int quantity);
        Task IncreaseStoreInventoryAsync(int storeNo, string itemNo, int quantity);
        Task DecreaseStoreInventoryAsync(int storeNo, string itemNo, int quantity);
        Task DescreaseDistributionCenterInventoryAsync(string itemNo, int quantity);
    }
}
