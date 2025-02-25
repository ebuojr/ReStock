namespace ReStockDomain
{
    public class SalesOrderLine
    {
        public int Id { get; set; }
        public string HeaderNo { get; set; }
        public int LineNo { get; set; }
        public string ProductNo { get; set; }
        public int Quantity { get; set; }
        public int ShippedQuantity { get; set; }
        public int ReceivedQuantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
