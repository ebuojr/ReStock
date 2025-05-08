using RestockWeb.Models;

namespace RestockWeb.Services.Store
{
    public interface IStoreService
    {
        Task CreateStoreAsync(Models.Store store);
        Task<List<Models.Store>?> GetAllStoresAsync();
        Task<Models.Store?> GetStoreAsync(int storeNo);
        Task UpdateStoreAsync(Models.Store store);
        Task DeleteStoreAsync(int id);
    }
}