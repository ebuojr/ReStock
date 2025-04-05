namespace RestockWeb.Models
{
    public class SalesOrder
    {
        public int Id { get; set; }
        public string HeaderNo { get; set; } = string.Empty;
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