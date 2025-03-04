
using ReStockApi.Models;

namespace ReStockApi.Services.SalesOrder
{
    public interface ISalesOrderService
    {
        Task<(Models.SalesOrder, IEnumerable<SalesOrderLine>)> CreateSalesOrderAsync(Models.SalesOrder salesOrder, List<SalesOrderLine> salesOrderLines);
        Task<(Models.SalesOrder, IEnumerable<SalesOrderLine>)> GetSalesOrderAsync(string headerNo);
        Task<Models.SalesOrder> UpdateSalesOrderAsync(Models.SalesOrder salesOrder);
    }
}
