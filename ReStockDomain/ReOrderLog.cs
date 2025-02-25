namespace ReStockDomain
{
    public class ReOrderLog
    {
        public int Id { get; set; }
        public int StoreNo { get; set; }
        public string ProductNo { get; set; }
        public int Quantity { get; set; }
        public DateTime LogTime { get; set; }
        public string EventType { get; set; }
        public string Description { get; set; }
        public bool Ordered { get; set; }
    }
}
