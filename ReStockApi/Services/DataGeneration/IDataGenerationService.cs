namespace ReStockApi.Services.DataGeneration
{
    public interface IDataGenerationService
    {
        Task TruncateTableByTableName(string tableName);
        Task GenerateProductItems();
        Task GenerateStoreinventory();
        Task GenerateDCInventory();
        Task GenereateInventoryThresholds();
    }
}
