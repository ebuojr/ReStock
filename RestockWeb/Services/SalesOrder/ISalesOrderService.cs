using RestockWeb.Models;

namespace RestockWeb.Services.SalesOrder
{
    public interface ISalesOrderService
    {
        Task<List<Models.SalesOrder>?> GetSalesOrdersAsync();
        Task<Models.SalesOrder?> GetSalesOrderAsync(string headerNo);
        Task<List<SalesOrderLine>?> GetSalesOrderLinesAsync(string headerNo);
        Task<bool> CreateSalesOrderAsync(Models.SalesOrder order, List<SalesOrderLine> orderLines);
        Task<bool> UpdateSalesOrderStatusAsync(string headerNo, OrderStatus status);
    }
}