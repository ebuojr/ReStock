using System.ComponentModel.DataAnnotations;

namespace ReStockDomain
{
    public class Store
    {
        [Key]
        public int Id { get; set; }
        public int No { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
    }
}
