using System.ComponentModel.DataAnnotations;

namespace ReStockApi.Models
{
    public class DistributionCenterInventory
    {
        [Key]
        public int Id { get; set; }
        public string ItemNo { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
