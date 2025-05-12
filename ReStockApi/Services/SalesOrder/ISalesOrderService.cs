
using ReStockApi.DTOs;

namespace ReStockApi.Services.SalesOrder
{
    public interface ISalesOrderService
    {
        Task CreateSalesOrderAsync(List<Models.Reorder> reorders);
        Task<List<SalesOrderDTO>> GetAllSalesOrdersAsync();
        Task<SalesOrderDTO> GetSalesOrderByHeaderNoAsync(string headerNo);
        Task<List<SalesOrderDTO>> GetSalesOrderByStoreNoAsync(int storeNo);
        Task<Models.SalesOrder> UpdateSalesOrderHeaderAsync(Models.SalesOrder salesOrder);
        Task<string> GetSalesOrderNumber(); 
    }
}
