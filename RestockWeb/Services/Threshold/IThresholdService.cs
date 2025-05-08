using RestockWeb.Models;

namespace RestockWeb.Services.Threshold
{
    public interface IThresholdService
    {
        Task<List<InventoryThreshold>?> GetAllThresholdsAsync();
        Task<InventoryThreshold?> GetThresholdAsync(int storeNo, string itemNo);
        Task<List<InventoryThreshold>?> GetThresholdsByStoreAsync(int storeNo);
        Task CreateThresholdAsync(InventoryThreshold threshold);
        Task UpdateThresholdAsync(InventoryThreshold threshold);
        Task DeleteThresholdAsync(int id);
    }
}