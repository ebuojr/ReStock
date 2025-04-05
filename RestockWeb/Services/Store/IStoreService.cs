using RestockWeb.Models;

namespace RestockWeb.Services.Store
{
    public interface IStoreService
    {
        Task<bool> CreateStoreAsync(Models.Store store);
        Task<List<Models.Store>?> GetAllStoresAsync();
        Task<Models.Store?> GetStoreAsync(int storeNo);
        Task<bool> UpdateStoreAsync(Models.Store store);
        Task<bool> DeleteStoreAsync(int id);
    }
}