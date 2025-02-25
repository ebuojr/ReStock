using ReStockDomain;

namespace ReStockService.Threshold
{
    public interface IThresholdService
    {
        Task<InventoryThreshold> GetThresholdAsync(int storeNo, string productNo);
        Task UpdateThresholdAsync(InventoryThreshold threshold);
    }
}
