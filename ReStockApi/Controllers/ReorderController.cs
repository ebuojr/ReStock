using Microsoft.AspNetCore.Mvc;
using ReStockApi.Models;
using ReStockApi.Services.Reorder;

namespace ReStockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReorderController : Controller
    {
        private readonly IReorderService _reorderService;
        public ReorderController(IReorderService reorderService)
        {
            _reorderService = reorderService;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessReorder([FromBody] Reorder reorder)
        {
            var result = await _reorderService.ProcessReorderAsync(reorder);
            return Ok(result);
        }
    }
}
