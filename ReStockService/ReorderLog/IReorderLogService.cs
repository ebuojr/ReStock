namespace ReStockService.ReorderLog
{
    public interface IReorderLogService
    {
        Task<IEnumerable<ReStockDomain.ReOrderLog>> GetLogsByProductNoProductNoAsync(int storeNo, string productNo);
        Task<IEnumerable<ReStockDomain.ReOrderLog>> GetLogsByStoreNoAsync(int storeNo);
        Task LogAsync(int storeNo, string productNo, int quantity, string eventType, string description, bool ordered);
    }
}
