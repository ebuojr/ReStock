using ReStockDomain;

namespace ReStockService.Threshold
{
    public interface IThresholdService
    {
        Task<InventoryThreshold> GetThresholdAsync(int storeNo, string ItemNo);
        Task CreateThreshold(InventoryThreshold threshold);
        Task UpdateThresholdAsync(InventoryThreshold threshold);
    }
}
