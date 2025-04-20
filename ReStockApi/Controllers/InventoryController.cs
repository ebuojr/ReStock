using Microsoft.AspNetCore.Mvc;
using ReStockApi.Services.Inventory;

namespace ReStockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _InventroyService;
        public InventoryController(IInventoryService inventroyService)
        {
            _InventroyService = inventroyService;
        }

        [HttpGet("store-inventory-by-store-no")]
        public async Task<IActionResult> GetStoreInventoryByStoreNo(int storeNo)
        {
            var items = await _InventroyService.GetStoreInventoryByStoreNoAsync(storeNo);
            return Ok(items);
        }

        [HttpGet("store-inventory-by-item")]
        public async Task<IActionResult> GetStoreInventory(int storeNo, string ItemNo)
        {
            var item = await _InventroyService.GetStoreInventoryAsync(storeNo, ItemNo);
            return Ok(item ?? null);
        }

        [HttpPut("update-store-inventory")]
        public async Task<ActionResult> UpdateStoreInventory([FromBody] Models.StoreInventory inventory)
        {
            await _InventroyService.UpdateStoreInventoryAsync(inventory);
            return Ok();
        }

        [HttpGet("distribution-center-inventory-by-item")]
        public async Task<IActionResult> GetDistributionCenterInventory(string ItemNo)
        {
            var item = await _InventroyService.GetDistributionCenterInventoryAsync(ItemNo);
            return Ok(item ?? null);
        }

        [HttpPut("distribution-center-inventory")]
        public async Task<ActionResult> UpdateDistributionCenterInventory([FromBody] Models.DistributionCenterInventory inventory)
        {
            await _InventroyService.UpdateDistributionCenterInventoryAsync(inventory);
            return Ok();
        }
    }
}
