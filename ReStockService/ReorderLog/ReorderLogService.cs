using Dapper.FastCrud;
using Microsoft.Data.SqlClient;
using ReStockDomain;
using System.Data;

namespace ReStockService.ReorderLog
{
    class ReorderLogService : IReorderLogService
    {
        private string _connectionString;
        public ReorderLogService(string connectionString)
        {
            _connectionString = connectionString;
        }
        private IDbConnection GetConnection() => new SqlConnection(_connectionString);

        public async Task LogAsync(int storeNo, string productNo, int quantity, string eventType, string description, bool ordered)
            => await GetConnection().InsertAsync<ReOrderLog>(new ReOrderLog()
            {
                StoreNo = storeNo,
                ProductNo = productNo,
                Quantity = quantity,
                EventType = eventType,
                Description = description,
                Ordered = ordered
            });

        public async Task<IEnumerable<ReOrderLog>> GetLogsByProductNoProductNoAsync(int storeNo, string productNo)
        {
            using var connection = GetConnection();
            var logs = await connection.FindAsync<ReOrderLog>(statement => statement
                .Where($"{nameof(ReOrderLog.StoreNo):C} = @StoreNo AND {nameof(ReOrderLog.ProductNo):C} = @ProductNo")
                .WithParameters(new { StoreNo = storeNo, ProductNo = productNo }));
            return logs;
        }

        public async Task<IEnumerable<ReOrderLog>> GetLogsByStoreNoAsync(int storeNo)
            => await GetConnection().FindAsync<ReOrderLog>(statement => statement
                .Where($"{nameof(ReOrderLog.StoreNo):C} = @StoreNo")
                .WithParameters(new { StoreNo = storeNo }));
    }
}
