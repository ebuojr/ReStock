using Microsoft.EntityFrameworkCore;
using ReStockDomain;

namespace ReStockService.SalesOrder
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly ReStockDbContext _db;

        public SalesOrderService(ReStockDbContext db)
        {
            _db = db;
        }

        public async Task<(ReStockDomain.SalesOrder, IEnumerable<SalesOrderLine>)> CreateSalesOrderAsync(ReStockDomain.SalesOrder salesOrder, List<SalesOrderLine> salesOrderLines)
        {
            await _db.SalesOrders.AddAsync(salesOrder);
            await _db.SalesOrderLines.AddRangeAsync(salesOrderLines);
            await _db.SaveChangesAsync();

            return (salesOrder, salesOrderLines);
        }

        public async Task<(ReStockDomain.SalesOrder, IEnumerable<SalesOrderLine>)> GetSalesOrderAsync(string headerNo)
        {
            var header = await _db.SalesOrders.FirstOrDefaultAsync(so => so.HeaderNo == headerNo);
            var lines = await _db.SalesOrderLines.Where(sol => sol.HeaderNo == headerNo).ToListAsync();

            return (header, lines);
        }

        public async Task<ReStockDomain.SalesOrder> UpdateSalesOrderAsync(ReStockDomain.SalesOrder salesOrder)
        {
            _db.SalesOrders.Update(salesOrder);
            await _db.SaveChangesAsync();

            return salesOrder;
        }
    }
}