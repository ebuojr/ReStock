using Microsoft.AspNetCore.Mvc;
using ReStockApi.Services.DataGeneration;

namespace ReStockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataGeneratorController : ControllerBase
    {
        private readonly IDataGenerationService _DataGenerationService;
        public DataGeneratorController(IDataGenerationService dataGenerationService)
        {
            _DataGenerationService = dataGenerationService;
        }

        [HttpPost("truncate-table")]
        public async Task<IActionResult> TruncateTable([FromQuery] string tableName)
        {
            try
            {
                await _DataGenerationService.TruncateTableByTableName(tableName);
                return Ok($"Table {tableName} truncated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error truncating table {tableName}: {ex.Message}");
            }
        }

        [HttpPost("generate-new-dataset")]
        public async Task<IActionResult> GenerateNewDataset()
        {
            try
            {
                await _DataGenerationService.TruncateTableByTableName("Products");
                await _DataGenerationService.TruncateTableByTableName("StoreInventories");
                await _DataGenerationService.TruncateTableByTableName("DistributionCenterInventories");
                await _DataGenerationService.TruncateTableByTableName("InventoryThresholds");
                await _DataGenerationService.TruncateTableByTableName("ReOrderLogs");
                await _DataGenerationService.TruncateTableByTableName("SalesOrders");
                await _DataGenerationService.TruncateTableByTableName("SalesOrderLines");

                await _DataGenerationService.GenerateProductItems();
                await _DataGenerationService.GenerateDCInventory();
                await _DataGenerationService.GenerateStoreinventory();
                await _DataGenerationService.GenereateInventoryThresholds();
                return Ok("New dataset generated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error generating new dataset: {ex.Message}");
            }
        }
    }
}
