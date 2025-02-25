namespace ReStockDomain
{
    public class Product
    {
        public int Id { get; set; }
        public string No { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
    }
}