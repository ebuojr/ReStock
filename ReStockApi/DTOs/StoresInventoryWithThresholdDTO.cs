namespace ReStockApi.DTOs
{
    public class StoresInventoryWithThresholdDTO
    {
        public int StoreNo { get; set; }
        public string ItemNo { get; set; }
        public int CurrentQuantity { get; set; }
        public int MinimumQuantity { get; set; }
        public int TargetQuantity { get; set; }
        public int ReorderQuantity { get; set; }
    }
}
