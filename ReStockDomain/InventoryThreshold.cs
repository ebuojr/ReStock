namespace ReStockDomain
{
    public class InventoryThreshold
    {
        public int Id { get; set; }
        public int StoreNo { get; set; }
        public string ProductNo { get; set; }
        public int MinimumQuantity { get; set; }
        public int TargetQuantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
