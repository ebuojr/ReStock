using Microsoft.AspNetCore.Mvc;
using ReStockApi.Services.Store;

namespace ReStockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;
        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllStores()
        {
            var items = await _storeService.GetAllStores();
            return Ok(items);
        }
        [HttpPost("create")]
        public async Task<ActionResult> CreateStore([FromBody] Models.Store store)
        {
            await _storeService.CreateNewStore(store);
            return Ok();
        }
        [HttpPut("update")]
        public async Task<ActionResult> UpdateStore([FromBody] Models.Store store)
        {
            await _storeService.UpdateStore(store);
            return Ok();
        }
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteStore(int id)
        {
            await _storeService.DeleteStore(id);
            return Ok();
        }

        [HttpGet("get/{StoreNo}")]
        public async Task<ActionResult> GetStoreByNo(int storeNo)
        {
            var item = await _storeService.GetStore(storeNo);
            return Ok(item ?? null);
        }
    }
}
