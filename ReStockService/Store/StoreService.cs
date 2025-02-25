
using Dapper.FastCrud;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ReStockService.Store
{
    public class StoreService : IStoreService
    {
        private readonly string _connectionString;
        private IDbConnection GetConnection() => new SqlConnection(_connectionString);

        public StoreService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task CreateNewStore(ReStockDomain.Store store)
            => await GetConnection().InsertAsync<ReStockDomain.Store>(store);

        public async Task<List<ReStockDomain.Store>> GetAllStores()
            => (await GetConnection().FindAsync<ReStockDomain.Store>()).ToList();

        public Task<ReStockDomain.Store> GetStore(int storeNo)
            => GetConnection().GetAsync<ReStockDomain.Store>(new ReStockDomain.Store() { No = storeNo });

        public Task UpdateStore(ReStockDomain.Store store)
            => GetConnection().UpdateAsync<ReStockDomain.Store>(store);
    }
}
