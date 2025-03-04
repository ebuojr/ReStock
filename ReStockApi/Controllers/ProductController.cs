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
            return Ok(_productService.GetProductsAsync());
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateProduct([FromBody] Models.Product product)
        {
            await _productService.CreateProductAsync(product);
            return Ok();
        }
    }
}
