using System.ComponentModel.DataAnnotations;

namespace ReStockApi.Models
{
    public class ReOrderLog
    {
        [Key]
        public int Id { get; set; }
        public int StoreNo { get; set; }
        public string ItemNo { get; set; }
        public int Quantity { get; set; }
        public DateTime LogTime { get; set; }
        public string EventType { get; set; }
        public string Description { get; set; }
        public bool Error { get; set; }
    }
}
