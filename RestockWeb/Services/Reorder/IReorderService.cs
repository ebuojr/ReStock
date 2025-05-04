using RestockWeb.Models;

namespace RestockWeb.Services.Reorder
{
    public interface IReorderService
    {
        Task<List<Models.Reorder>> CreatePotentialOrdersByStoreNoAsync(int storeNo);
        Task<bool> ProcessReorderAsync(List<Models.Reorder> reorders);
    }
}