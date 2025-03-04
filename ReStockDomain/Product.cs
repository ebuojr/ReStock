using System.ComponentModel.DataAnnotations;

namespace ReStockDomain
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string ItemNo { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal RetailPrice { get; set; }
        public bool IsActive { get; set; }
    }
}