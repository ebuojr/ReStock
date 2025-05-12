using Microsoft.EntityFrameworkCore;
using ReStockApi.DTOs;
using ReStockApi.Models;
using ReStockApi.Services.Inventory;

namespace ReStockApi.Services.SalesOrder
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly ReStockDbContext _db;
        private readonly IInventoryService _inventoryService;

        public SalesOrderService(ReStockDbContext db, IInventoryService inventoryService)
        {
            _db = db;
            _inventoryService = inventoryService;
        }

        public async Task CreateSalesOrderAsync(List<Models.Reorder> reorder)
        {
            var salesorderlines = new List<SalesOrderLine>();

            var stores = reorder.Where(x => x.StoreNo != 0).Select(x => x.StoreNo).Distinct().ToList();
            foreach (var store in stores)
            {
                var currentReorders = reorder.Where(reorder => reorder.StoreNo == store).ToList();
                var storeReorders = reorder.Where(x => x.StoreNo == store).ToList();
                var salesOrder = new Models.SalesOrder
                {
                    StoreNo = store,
                    HeaderNo = $"SO-{await GetSalesOrderNumber()}",
                    OrderStatus = OrderStatus.Shipped,
                    OrderDate = DateTime.UtcNow
                };

                int lineNo = 100;
                foreach (var item in storeReorders)
                {
                    salesorderlines.Add(new SalesOrderLine
                    {
                        HeaderNo = salesOrder.HeaderNo,
                        LineNo = lineNo++,
                        ItemNo = item.ItemNo,
                        Quantity = await _inventoryService.CheckAvailabilityAsync(item.ItemNo, item.Quantity)
                    });
                }

                await _db.SalesOrders.AddAsync(salesOrder);
                
                foreach (var line in salesorderlines)
                {
                    await _inventoryService.IncreaseStoreInventoryAsync(store, line.ItemNo, line.Quantity);
                    await _inventoryService.DescreaseDistributionCenterInventoryAsync(line.ItemNo, line.Quantity);
                    await _db.SalesOrderLines.AddAsync(line);
                }

                _db.SaveChanges();
            }
        }

        public async Task<List<SalesOrderDTO>> GetAllSalesOrdersAsync()
        {
            var result = new List<SalesOrderDTO>();
            var headers = await _db.SalesOrders.AsNoTracking().ToListAsync();
            foreach (var header in headers)
            {
                var lines = await _db.SalesOrderLines.AsNoTracking().Where(sol => sol.HeaderNo == header.HeaderNo).ToListAsync();
                result.Add(new SalesOrderDTO() { SalesOrder = header, SalesOrderLines = lines });
            }

            return result.OrderByDescending(so => so.SalesOrder.OrderDate).ToList();
        }

        public async Task<SalesOrderDTO> GetSalesOrderByHeaderNoAsync(string headerNo)
        {
            var header = await _db.SalesOrders.FirstOrDefaultAsync(so => so.HeaderNo == headerNo);
            var lines = await _db.SalesOrderLines.Where(sol => sol.HeaderNo == headerNo).ToListAsync();

            return new SalesOrderDTO() { SalesOrder = header, SalesOrderLines = lines };
        }

        public async Task<List<SalesOrderDTO>> GetSalesOrderByStoreNoAsync(int storeNo)
        {
            var result = new List<SalesOrderDTO>();

            var headers = await _db.SalesOrders.Where(so => so.StoreNo == storeNo).ToListAsync();
            foreach (var header in headers)
            {
                var lines = await _db.SalesOrderLines.Where(sol => sol.HeaderNo == header.HeaderNo).ToListAsync();
                result.Add(new SalesOrderDTO() { SalesOrder = header, SalesOrderLines = lines });
            }

            return result;
        }

        public async Task<string> GetSalesOrderNumber()
        {
            var currentNo = await _db.SalesOrderNumber.FirstAsync();
            currentNo.No += 1;
            _db.SalesOrderNumber.Update(currentNo);
            await _db.SaveChangesAsync();
            return currentNo.No.ToString();
        }

        public async Task<Models.SalesOrder> UpdateSalesOrderHeaderAsync(Models.SalesOrder salesOrder)
        {
            _db.SalesOrders.Update(salesOrder);
            await _db.SaveChangesAsync();

            return salesOrder;
        }
    }
}