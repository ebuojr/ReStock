using ReStockApi.DTOs;

namespace ReStockApi.Services.SalesOrder
{
    public interface ISalesOrderService
    {
        /// <summary>
        /// Creates sales orders and updates inventory based on the provided reorders.
        /// </summary>
        /// <param name="reorders">The list of reorders to process into sales orders.</param>
        Task CreateSalesOrderAsync(List<Models.Reorder> reorders);

        /// <summary>
        /// Gets all sales orders with their lines.
        /// </summary>
        /// <returns>A list of sales order DTOs.</returns>
        Task<List<SalesOrderDTO>> GetAllSalesOrdersAsync();

        /// <summary>
        /// Gets a sales order and its lines by header number.
        /// </summary>
        /// <param name="headerNo">The header number of the sales order.</param>
        /// <returns>The sales order DTO.</returns>
        Task<SalesOrderDTO> GetSalesOrderByHeaderNoAsync(string headerNo);

        /// <summary>
        /// Gets all sales orders and their lines for a specific store.
        /// </summary>
        /// <param name="storeNo">The store number.</param>
        /// <returns>A list of sales order DTOs for the store.</returns>
        Task<List<SalesOrderDTO>> GetSalesOrderByStoreNoAsync(int storeNo);

        /// <summary>
        /// Updates the header of a sales order.
        /// </summary>
        /// <param name="salesOrder">The sales order to update.</param>
        /// <returns>The updated sales order.</returns>
        Task<Models.SalesOrder> UpdateSalesOrderHeaderAsync(Models.SalesOrder salesOrder);

        /// <summary>
        /// Gets the next sales order number and increments the counter.
        /// </summary>
        /// <returns>The next sales order number as a string.</returns>
        Task<string> GetSalesOrderNumber();
    }
}
