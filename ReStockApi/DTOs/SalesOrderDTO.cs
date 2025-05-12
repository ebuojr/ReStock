using ReStockApi.Models;

namespace ReStockApi.DTOs
{
    public class SalesOrderDTO
    {
        public SalesOrder SalesOrder { get; set; }
        public List<SalesOrderLine> SalesOrderLines { get; set; }
    }
}
