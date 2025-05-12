using Microsoft.AspNetCore.Mvc;
using ReStockApi.Models;
using ReStockApi.Services.SalesOrder;

namespace ReStockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderController : ControllerBase
    {
        private readonly ISalesOrderService _salesOrderService;
        public SalesOrderController(ISalesOrderService salesOrderService)
        {
            _salesOrderService = salesOrderService;
        }

        [HttpPost("craete-sales-order")]
        public async Task<IActionResult> CreateSalesOrder([FromBody] List<Reorder> reorders)
        {
            await _salesOrderService.CreateSalesOrderAsync(reorders);
            return Ok();
        }

        [HttpGet("get-sales-order-by-headerNo/{headerNo}")]
        public async Task<IActionResult> GetSalesOrderByHeaderNo(string headerNo)
        {
            var result = await _salesOrderService.GetSalesOrderByHeaderNoAsync(headerNo);
            return Ok(result);
        }

        [HttpGet("get-sales-order-by-storeNo/{storeNo}")]
        public async Task<IActionResult> GetSalesOrderByStoreNo(int storeNo)
        {
            var result = await _salesOrderService.GetSalesOrderByStoreNoAsync(storeNo);
            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllSalesOrders()
        {
            var result = await _salesOrderService.GetAllSalesOrdersAsync();
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSalesOrderHeader([FromBody] Models.SalesOrder salesOrder)
        {
            var result = await _salesOrderService.UpdateSalesOrderHeaderAsync(salesOrder);
            return Ok(result);
        }
    }
}
