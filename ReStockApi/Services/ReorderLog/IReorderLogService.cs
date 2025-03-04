using ReStockApi.Models;

namespace ReStockApi.Services.ReorderLog
{
    public interface IReorderLogService
    {
        Task<IEnumerable<ReOrderLog>> GetLogsByItemNoItemNoAsync(int storeNo, string ItemNo);
        Task<IEnumerable<ReOrderLog>> GetLogsByStoreNoAsync(int storeNo);
        Task LogAsync(int storeNo, string ItemNo, int quantity, string eventType, string description, bool ordered);
    }
}
