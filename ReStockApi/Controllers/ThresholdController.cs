using Microsoft.AspNetCore.Mvc;
using ReStockApi.Services.Threshold;

namespace ReStockApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThresholdController : ControllerBase
    {
        private readonly IThresholdService _thresholdService;
        public ThresholdController(IThresholdService thresholdService)
        {
            _thresholdService = thresholdService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetThresholds()
        {
            var threadholds = await _thresholdService.GetThresholdsAsync();
            return Ok(threadholds ?? null);
        }

        [HttpGet("get-by-store-item")]
        public async Task<IActionResult> GetThreshold(int storeNo, string ItemNo)
        {
            var threshold = await _thresholdService.GetThresholdAsync(storeNo, ItemNo);
            return Ok(threshold ?? null);
        }

        [HttpGet("storeno")]
        public async Task<IActionResult> GetThresholdsByStoreNo(int storeNo)
        {
            var thresholds = await _thresholdService.GetThresholdsByStoreNoAsync(storeNo);
            return Ok(thresholds);
        }

        [HttpPost]
        public async Task<ActionResult> CreateThreshold([FromBody] Models.InventoryThreshold threshold)
        {
            await _thresholdService.CreateThreshold(threshold);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateThreshold([FromBody] Models.InventoryThreshold threshold)
        {
            await _thresholdService.UpdateThresholdAsync(threshold);
            return Ok();
        }
    }
}
