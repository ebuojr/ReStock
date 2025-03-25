
using ReStockApi.Models;

namespace ReStockApi.Services.SalesOrder
{
    public interface ISalesOrderService
    {
        Task<(Models.SalesOrder, IEnumerable<SalesOrderLine>)> CreateSalesOrderAsync(Models.SalesOrder salesOrder, List<SalesOrderLine> salesOrderLines);
        Task<(Models.SalesOrder, IEnumerable<SalesOrderLine>)> GetSalesOrderByHeaderNoAsync(string headerNo);
        Task<(Models.SalesOrder, IEnumerable<SalesOrderLine>)> GetSalesOrderByStoreNoAsync(int storeNo);
        Task<Models.SalesOrder> UpdateSalesOrderHeaderAsync(Models.SalesOrder salesOrder);
    }
}
