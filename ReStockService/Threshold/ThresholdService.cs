using Dapper.FastCrud;
using Microsoft.Data.SqlClient;
using ReStockDomain;
using System.Data;

namespace ReStockService.Threshold
{
    public class ThresholdService : IThresholdService
    {
        private string _connectionString;
        public ThresholdService(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection GetConnection() => new SqlConnection(_connectionString);

        public async Task<InventoryThreshold> GetThresholdAsync(int storeNo, string productNo)
            => await GetConnection().GetAsync<InventoryThreshold>(new InventoryThreshold() { StoreNo = storeNo, ProductNo = productNo });

        public async Task UpdateThresholdAsync(InventoryThreshold threshold)
            => await GetConnection().UpdateAsync(threshold);
    }
}
