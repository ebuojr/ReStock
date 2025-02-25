using Dapper.FastCrud;
using Microsoft.Data.SqlClient;
using ReStockDomain;
using System.Data;

namespace ReStockService.Inventory
{
    public class InventoryService : IInventoryService
    {
        private string _connectionString;
        public InventoryService(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection GetConnection() => new SqlConnection(_connectionString);

        public async Task<DistributionCenterInventory> GetDistributionCenterInventoryAsync(string productNo)
            => await GetConnection().GetAsync<DistributionCenterInventory>(new DistributionCenterInventory() { ProductNo = productNo });

        public async Task<StoreInventory> GetStoreInventoryAsync(int storeNo, string productNo)
            => await GetConnection().GetAsync<StoreInventory>(new StoreInventory() { StoreNo = storeNo, ProductNo = productNo });

        public async Task UpdateDistributionCenterInventoryAsync(DistributionCenterInventory inventory)
            => await GetConnection().UpdateAsync(inventory);

        public Task UpdateStoreInventoryAsync(StoreInventory inventory)
            => GetConnection().UpdateAsync(inventory);
    }
}
