using System.ComponentModel.DataAnnotations;

namespace ReStockApi.Models
{
    public class SalesOrderNumber
    {
        [Key]
        public int Id { get; set; }
        public int No { get; set; }
    }
}
