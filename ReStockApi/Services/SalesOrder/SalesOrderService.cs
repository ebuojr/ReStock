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

        /// <summary>
        /// Initializes a new instance of the SalesOrderService class.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="inventoryService">The inventory service.</param>
        public SalesOrderService(ReStockDbContext db, IInventoryService inventoryService)
        {
            _db = db;
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// Creates sales orders and updates inventory based on the provided reorders.
        /// </summary>
        /// <param name="reorder">The list of reorders to process into sales orders.</param>
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

        /// <summary>
        /// Gets all sales orders with their lines.
        /// </summary>
        /// <returns>A list of sales order DTOs.</returns>
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

        /// <summary>
        /// Gets a sales order and its lines by header number.
        /// </summary>
        /// <param name="headerNo">The header number of the sales order.</param>
        /// <returns>The sales order DTO.</returns>
        public async Task<SalesOrderDTO> GetSalesOrderByHeaderNoAsync(string headerNo)
        {
            var header = await _db.SalesOrders.FirstOrDefaultAsync(so => so.HeaderNo == headerNo);
            var lines = await _db.SalesOrderLines.Where(sol => sol.HeaderNo == headerNo).ToListAsync();

            return new SalesOrderDTO() { SalesOrder = header, SalesOrderLines = lines };
        }

        /// <summary>
        /// Gets all sales orders and their lines for a specific store.
        /// </summary>
        /// <param name="storeNo">The store number.</param>
        /// <returns>A list of sales order DTOs for the store.</returns>
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

        /// <summary>
        /// Gets the next sales order number and increments the counter.
        /// </summary>
        /// <returns>The next sales order number as a string.</returns>
        public async Task<string> GetSalesOrderNumber()
        {
            var currentNo = await _db.SalesOrderNumber.FirstAsync();
            currentNo.No += 1;
            _db.SalesOrderNumber.Update(currentNo);
            await _db.SaveChangesAsync();
            return currentNo.No.ToString();
        }

        /// <summary>
        /// Updates the header of a sales order.
        /// </summary>
        /// <param name="salesOrder">The sales order to update.</param>
        /// <returns>The updated sales order.</returns>
        public async Task<Models.SalesOrder> UpdateSalesOrderHeaderAsync(Models.SalesOrder salesOrder)
        {
            _db.SalesOrders.Update(salesOrder);
            await _db.SaveChangesAsync();

            return salesOrder;
        }
    }
}