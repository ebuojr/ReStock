using ReStockApi.Services.Inventory;
using ReStockApi.Services.Product;
using ReStockApi.Services.Reorder;
using ReStockApi.Services.ReorderLog;
using ReStockApi.Services.Store;
using System.Security.Cryptography;

namespace ReStockApiTest
{
    
    public class ReorderServiceTest
    {
        private readonly IReorderService _reorderService;
        private readonly IReorderLogService _reorderLogService;
        private readonly IProductService _productService;
        private readonly IStoreService _storeService;
        private readonly IInventoryService _inventoryService;

        public ReorderServiceTest(IReorderService reorderService, IReorderLogService reorderLogService, IProductService productService, IStoreService storeService, IInventoryService inventoryService)
        {
            _reorderService = reorderService;
            _reorderLogService = reorderLogService;
            _productService = productService;
            _storeService = storeService;
            _inventoryService = inventoryService;
        }

        [Fact]
        public async Task CreatePotentialOrdersByStoreNoAsync_StoreDoesNotExists()
        {

        }
    }
}
