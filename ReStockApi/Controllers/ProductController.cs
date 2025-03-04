using Microsoft.AspNetCore.Mvc;
using ReStockApi.Services.Product;

namespace ReStockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var items = await _productService.GetProductsAsync();
            return Ok(items);
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateProduct([FromBody] Models.Product product)
        {
            await _productService.CreateProductAsync(product);
            return Ok();
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateProduct([FromBody] Models.Product product)
        {
            await _productService.UpdateProductAsync(product);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok();
        }

        [HttpGet("get/{ItemNo}")]
        public async Task<ActionResult> GetProductByNo(string ItemNo)
        {
            var item = await _productService.GetProductByNoAsync(ItemNo);
            return Ok(item ?? null);
        }
    }
}
