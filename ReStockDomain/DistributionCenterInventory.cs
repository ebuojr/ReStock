namespace ReStockDomain
{
    public class DistributionCenterInventory
    {
        public int Id { get; set; }
        public string ProductNo { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
