using System.ComponentModel.DataAnnotations;

namespace ReStockApi.Models
{
    public class Reorder
    {
        [Key]
        public int Id { get; set; }
        public int StoreNo { get; set; }
        public string ItemNo { get; set; }
        public int Quantity { get; set; }
    }
}
