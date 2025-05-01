

namespace ReStockApi.Services.Reorder
{
    public interface IReorderService
    {
        Task<List<Models.Reorder>> CreatePotentialOrdersByStoreNoAsync(int storeNo);
        Task ProcessReorderAsync(List<Models.Reorder> reorders);
        Task InsertReorderAsync(List<Models.Reorder> reorders);
    }
}
