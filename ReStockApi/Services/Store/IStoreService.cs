namespace ReStockApi.Services.Store
{
    public interface IStoreService
    {
        Task CreateNewStore(Models.Store store);
        Task<List<Models.Store>> GetAllStores();
        Task<Models.Store> GetStore(int storeNo);
        Task UpdateStore(Models.Store store);
        Task DeleteStore(int id);
        Task StoreExists(int storeNo);
    }
}
