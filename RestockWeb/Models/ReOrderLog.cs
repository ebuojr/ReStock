namespace RestockWeb.Models
{
    public class ReOrderLog
    {
        public int Id { get; set; }
        public int StoreNo { get; set; }
        public string ItemNo { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime LogTime { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Error { get; set; }
    }
}