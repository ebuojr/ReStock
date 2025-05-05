using RestockWeb.Models;

namespace RestockWeb.Services.Reorderlogs
{
    public interface IReorderLogService
    {
        Task<IEnumerable<ReOrderLog>> GetLogsAsync(DateTime fromdate, string type, string no, string storeNo);
        Task LogAsync(int storeNo, string ItemNo, int quantity, string eventType, string description, bool ordered);
    }
}
