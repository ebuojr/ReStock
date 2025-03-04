using System.ComponentModel.DataAnnotations;

namespace ReStockApi.Models
{
    public class SalesOrderLine
    {
        [Key]
        public int Id { get; set; }
        public string HeaderNo { get; set; }
        public int LineNo { get; set; }
        public string ItemNo { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
