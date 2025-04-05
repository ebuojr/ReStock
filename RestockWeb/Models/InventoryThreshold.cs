namespace RestockWeb.Models
{
    public class InventoryThreshold
    {
        public int Id { get; set; }
        public int StoreNo { get; set; }
        public string ItemNo { get; set; } = string.Empty;
        public int MinimumQuantity { get; set; }
        public int TargetQuantity { get; set; }
        public int ReorderQuantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}