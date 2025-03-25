using ReStockApi.Models;

namespace ReStockApi.DTOs
{
    public class CreateSalesOrderDTO
    {
        public SalesOrder SalesOrder { get; set; }
        public List<SalesOrderLine> SalesOrderLines { get; set; }
    }
}
