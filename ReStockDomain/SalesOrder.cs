namespace ReStockDomain
{
    public class SalesOrder
    {
        public int Id { get; set; }
        public string HeaderNo { get; set; }
        public int StoreNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalInclVat { get; set; }
    }
}
