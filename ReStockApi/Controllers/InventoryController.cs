using Microsoft.AspNetCore.Mvc;
using ReStockApi.Services.Inventory;

namespace ReStockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _InventoryService;
        public InventoryController(IInventoryService inventroyService)
        {
            _InventoryService = inventroyService;
        }

        [HttpGet("store-inventory-with-Threshold-store-no")]
        public async Task<IActionResult> GetStoreInventoryByStoreNoWithThresholds(int storeNo)
        {
            var items = await _InventoryService.GetStoreInventoryByStoreNoWithThresholdsAsync(storeNo);
            return Ok(items);
        }

        [HttpGet("store")]
        public async Task<IActionResult> GetStoreInventoryByStoreNo(int storeNo)
        {
            var items = await _InventoryService.GetStoreInventoryByStoreNoAsync(storeNo);
            return Ok(items);
        }

        [HttpGet("store-item")]
        public async Task<IActionResult> GetStoreInventory(int storeNo, string ItemNo)
        {
            var item = await _InventoryService.GetStoreInventoryAsync(storeNo, ItemNo);
            return Ok(item ?? null);
        }

        [HttpPut("update-store-inventory")]
        public async Task<ActionResult> UpdateStoreInventory([FromBody] Models.StoreInventory inventory)
        {
            await _InventoryService.UpsertStoreInventoryAsync(inventory);
            return Ok();
        }

        [HttpGet("distribution-center-inventory")]
        public async Task<IActionResult> GetDistributionCenterInventory()
        {
            var items = await _InventoryService.GetDistributionCenterInventoryAsync();
            return Ok(items);
        }

        [HttpGet("distribution-center-inventory-by-item")]
        public async Task<IActionResult> GetDistributionCenterInventory(string ItemNo)
        {
            var item = await _InventoryService.GetDistributionCenterInventoryAsync(ItemNo);
            return Ok(item ?? null);
        }

        [HttpPut("distribution-center-inventory")]
        public async Task<ActionResult> UpdateDistributionCenterInventory([FromBody] Models.DistributionCenterInventory inventory)
        {
            await _InventoryService.UpsertDistributionCenterInventoryAsync(inventory);
            return Ok();
        }
    }
}
