using ReStockApi.Models;

namespace ReStockApi.Services.Threshold
{
    public interface IThresholdService
    {
        Task<InventoryThreshold> GetThresholdAsync(int storeNo, string ItemNo);
        Task CreateThreshold(InventoryThreshold threshold);
        Task UpdateThresholdAsync(InventoryThreshold threshold);
    }
}
