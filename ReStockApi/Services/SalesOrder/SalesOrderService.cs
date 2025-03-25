using Microsoft.EntityFrameworkCore;
using ReStockApi.Models;

namespace ReStockApi.Services.SalesOrder
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly ReStockDbContext _db;

        public SalesOrderService(ReStockDbContext db)
        {
            _db = db;
        }

        public async Task<(Models.SalesOrder, IEnumerable<SalesOrderLine>)> CreateSalesOrderAsync(Models.SalesOrder salesOrder, List<SalesOrderLine> salesOrderLines)
        {
            await _db.SalesOrders.AddAsync(salesOrder);
            await _db.SalesOrderLines.AddRangeAsync(salesOrderLines);
            await _db.SaveChangesAsync();

            return (salesOrder, salesOrderLines);
        }

        public async Task<(Models.SalesOrder, IEnumerable<SalesOrderLine>)> GetSalesOrderByHeaderNoAsync(string headerNo)
        {
            var header = await _db.SalesOrders.FirstOrDefaultAsync(so => so.HeaderNo == headerNo);
            var lines = await _db.SalesOrderLines.Where(sol => sol.HeaderNo == headerNo).ToListAsync();

            return (header, lines);
        }

        public async Task<(Models.SalesOrder, IEnumerable<SalesOrderLine>)> GetSalesOrderByStoreNoAsync(int storeNo)
        {
            var header = await _db.SalesOrders.FirstOrDefaultAsync(so => so.StoreNo == storeNo);
            var lines = await _db.SalesOrderLines.Where(sol => sol.HeaderNo == header.HeaderNo).ToListAsync();
            return (header, lines);
        }

        public async Task<Models.SalesOrder> UpdateSalesOrderHeaderAsync(Models.SalesOrder salesOrder)
        {
            _db.SalesOrders.Update(salesOrder);
            await _db.SaveChangesAsync();

            return salesOrder;
        }
    }
}