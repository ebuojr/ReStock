using RestockWeb.Models;

namespace RestockWeb.DTOs
{
    public class SalesOrderDTO
    {
        public SalesOrder SalesOrder { get; set; }
        public List<SalesOrderLine> SalesOrderLines { get; set; }
    }
}
