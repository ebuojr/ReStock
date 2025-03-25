using ReStockApi.Models;

namespace ReStockApi.Services.Reorder
{
    public interface IReorderService
    {
        Task<bool> ProcessReorderAsync(Models.Reorder reorder);
    }
}
