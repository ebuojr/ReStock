using System.ComponentModel.DataAnnotations;

namespace ReStockApi.Models
{
    public class InventoryThreshold
    {
        [Key]
        public int Id { get; set; }
        public int StoreNo { get; set; }
        public string ItemNo { get; set; }
        public int MinimumQuantity { get; set; }
        public int TargetQuantity { get; set; }
        public int ReorderQuantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
