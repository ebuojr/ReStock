using Microsoft.AspNetCore.Mvc;
using ReStockApi.Services.ReorderLog;

namespace ReStockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReorderlogController : ControllerBase
    {
        private readonly IReorderLogService reorderLogService;

        public ReorderlogController(IReorderLogService reorderLogService)
        {
            this.reorderLogService = reorderLogService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetLogs([FromQuery] DateTime? fromdate, string? type, string? no, string? storeNo)
        {
            return Ok(await reorderLogService.GetLogsAsync(fromdate.Value, type, no, storeNo));
        }
    }
}
