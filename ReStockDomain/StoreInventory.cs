namespace ReStockDomain
{
    public class StoreInventory
    {
        public int Id { get; set; }
        public int StoreNo { get; set; }
        public string ProductNo { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
