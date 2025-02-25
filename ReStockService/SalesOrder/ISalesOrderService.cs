using ReStockDomain;

namespace ReStockService.SalesOrder
{
    public interface ISalesOrderService
    {
        Task<(ReStockDomain.SalesOrder, IEnumerable<SalesOrderLine>)> CreateSalesOrderAsync(ReStockDomain.SalesOrder salesOrder, List<SalesOrderLine> salesOrderLines);
        Task<(ReStockDomain.SalesOrder, IEnumerable<SalesOrderLine>)> GetSalesOrderAsync(string headerNo);
        Task<ReStockDomain.SalesOrder> UpdateSalesOrderAsync(ReStockDomain.SalesOrder salesOrder);
    }
}
