using Dapper.FastCrud;
using Microsoft.Data.SqlClient;
using ReStockDomain;
using System.Data;

namespace ReStockService.SalesOrder
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly string _connectionString;
        private IDbConnection GetConnection() => new SqlConnection(_connectionString);
        public SalesOrderService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<(ReStockDomain.SalesOrder, IEnumerable<SalesOrderLine>)> CreateSalesOrderAsync(ReStockDomain.SalesOrder salesOrder, List<SalesOrderLine> salesOrderLines)
        {
            await GetConnection().InsertAsync(salesOrder);
            await GetConnection().InsertAsync(salesOrderLines);
            return (salesOrder, salesOrderLines);
        }

        public async Task<(ReStockDomain.SalesOrder, IEnumerable<SalesOrderLine>)> GetSalesOrderAsync(string headerNo)
        {
            var header = await GetConnection().GetAsync<ReStockDomain.SalesOrder>(new ReStockDomain.SalesOrder() { HeaderNo = headerNo });
            var lines = await GetConnection().FindAsync<SalesOrderLine>(statement => statement
                .Where($"{nameof(SalesOrderLine.HeaderNo):C} = @HeaderNo")
                .WithParameters(new { HeaderNo = headerNo }));

            return (header, lines);
        }

        public async Task<ReStockDomain.SalesOrder> UpdateSalesOrderAsync(ReStockDomain.SalesOrder salesOrder)
        {
            await GetConnection().UpdateAsync(salesOrder);
            return salesOrder;
        }
    }
}
