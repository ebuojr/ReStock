using System.ComponentModel.DataAnnotations;

namespace ReStockApi.Models
{
    public class JobLastRun
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime LastRunTime { get; set; }
    }
}
