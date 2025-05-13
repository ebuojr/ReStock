using RestockWeb.Models;

namespace RestockWeb.Services.Reorder
{
    public interface IReorderService
    {
        Task<List<Models.Reorder>> CreatePotentialOrdersByStoreNoAsync(int storeNo);
    }
}