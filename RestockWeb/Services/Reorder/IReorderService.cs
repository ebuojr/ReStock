using RestockWeb.Models;

namespace RestockWeb.Services.Reorder
{
    public interface IReorderService
    {
        Task<List<Models.Reorder>?> GetReordersAsync();
        Task<Models.Reorder?> GetReorderAsync(int id);
        Task<bool> CreateReorderAsync(Models.Reorder reorder);
        Task<bool> ProcessReorderAsync(int id);
        Task<List<ReOrderLog>?> GetReorderLogsAsync();
    }
}