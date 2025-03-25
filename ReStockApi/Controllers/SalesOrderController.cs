using Microsoft.AspNetCore.Mvc;
using ReStockApi.DTOs;
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
        public async Task<IActionResult> CreateSalesOrder([FromBody] CreateSalesOrderDTO salesOrder)
        {
            var result = await _salesOrderService.CreateSalesOrderAsync(salesOrder.SalesOrder, salesOrder.SalesOrderLines);
            return Ok(result);
        }

        [HttpGet("get-sales-order-by-headerNo")]
        public async Task<IActionResult> GetSalesOrderByHeaderNo(string headerNo)
        {
            var result = await _salesOrderService.GetSalesOrderByHeaderNoAsync(headerNo);
            return Ok(result);
        }

        [HttpGet("get-sales-order-by-storeNo")]
        public async Task<IActionResult> GetSalesOrderByStoreNo(int storeNo)
        {
            var result = await _salesOrderService.GetSalesOrderByStoreNoAsync(storeNo);
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
