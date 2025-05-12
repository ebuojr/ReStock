namespace RestockWeb.Models
{
    public class SalesOrderLine
    {
        public int Id { get; set; }
        public string HeaderNo { get; set; } = string.Empty;
        public int LineNo { get; set; }
        public string ItemNo { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}