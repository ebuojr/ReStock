namespace RestockWeb.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ItemNo { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal RetailPrice { get; set; }
        public bool IsActive { get; set; }
    }
}