using System.ComponentModel.DataAnnotations;

namespace ReStockApi.Models
{
    public class SalesOrder
    {
        [Key]
        public int Id { get; set; }
        public string HeaderNo { get; set; }
        public int StoreNo { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }

    public enum OrderStatus
    {
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}
