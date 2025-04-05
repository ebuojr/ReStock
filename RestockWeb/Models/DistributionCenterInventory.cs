namespace RestockWeb.Models
{
    public class DistributionCenterInventory
    {
        public int Id { get; set; }
        public string ItemNo { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}