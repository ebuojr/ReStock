namespace ReStockService.Store
{
    public interface IStoreService
    {
        Task CreateNewStore(ReStockDomain.Store store);
        Task<List<ReStockDomain.Store>> GetAllStores();
        Task<ReStockDomain.Store> GetStore(int storeNo);
        Task UpdateStore(ReStockDomain.Store store);
    }
}
