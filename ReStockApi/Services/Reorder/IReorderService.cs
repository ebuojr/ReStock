

namespace ReStockApi.Services.Reorder
{
    public interface IReorderService
    {
        Task<List<Models.Reorder>> CreatePotentialOrdersByStoreNoAsync(int storeNo);
        Task<bool> ProcessReorderAsync(Models.Reorder reorder);
    }
}
