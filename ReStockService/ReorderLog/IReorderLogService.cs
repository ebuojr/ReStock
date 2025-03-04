namespace ReStockService.ReorderLog
{
    public interface IReorderLogService
    {
        Task<IEnumerable<ReStockDomain.ReOrderLog>> GetLogsByItemNoItemNoAsync(int storeNo, string ItemNo);
        Task<IEnumerable<ReStockDomain.ReOrderLog>> GetLogsByStoreNoAsync(int storeNo);
        Task LogAsync(int storeNo, string ItemNo, int quantity, string eventType, string description, bool ordered);
    }
}
