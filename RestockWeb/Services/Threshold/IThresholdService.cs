using RestockWeb.Models;

namespace RestockWeb.Services.Threshold
{
    public interface IThresholdService
    {
        Task<List<InventoryThreshold>?> GetAllThresholdsAsync();
        Task<InventoryThreshold?> GetThresholdAsync(int storeNo, string itemNo);
        Task<List<InventoryThreshold>?> GetThresholdsByStoreAsync(int storeNo);
        Task<bool> CreateThresholdAsync(InventoryThreshold threshold);
        Task<bool> UpdateThresholdAsync(InventoryThreshold threshold);
        Task<bool> DeleteThresholdAsync(int id);
    }
}