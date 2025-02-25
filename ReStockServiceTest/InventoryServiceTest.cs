using Moq;
using ReStockDomain;
using ReStockService.Inventory;
using System.Data;

namespace ReStockServiceTest
{
    public class InventoryServiceTest
    {
        private readonly Mock<IInventoryService> _mockInventoryService;

        public InventoryServiceTest()
        {
            _mockInventoryService = new Mock<IInventoryService>();
        }

        [Fact]
        public async Task GetStoreInventory_By_StoreNo_ProductNo()
        {
            // Arrange
            var storeNo = 1;
            var productNo = "P1";
            var expectedInventory = new StoreInventory
            {
                StoreNo = storeNo,
                ProductNo = productNo,
                Quantity = 10,
                LastUpdated = DateTime.Now
            };

            _mockInventoryService
                .Setup(x => x.GetStoreInventoryAsync(storeNo, productNo))
                .ReturnsAsync(expectedInventory);

            // Act
            var result = await _mockInventoryService.Object.GetStoreInventoryAsync(storeNo, productNo);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedInventory.StoreNo, result.StoreNo);
            Assert.Equal(expectedInventory.ProductNo, result.ProductNo);
            Assert.Equal(expectedInventory.Quantity, result.Quantity);
        }

        [Fact]
        public async Task GetDistributionCenterInventory_By_ProductNo()
        {
            // Arrange
            var productNo = "P1";
            var expectedInventory = new DistributionCenterInventory
            {
                ProductNo = productNo,
                Quantity = 100,
                LastUpdated = DateTime.Now
            };
            _mockInventoryService
                .Setup(x => x.GetDistributionCenterInventoryAsync(productNo))
                .ReturnsAsync(expectedInventory);
            // Act
            var result = await _mockInventoryService.Object.GetDistributionCenterInventoryAsync(productNo);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedInventory.ProductNo, result.ProductNo);
            Assert.Equal(expectedInventory.Quantity, result.Quantity);
        }

        [Fact]
        public async Task UpdateStoreInventory()
        {
            // Arrange
            var inventory = new StoreInventory
            {
                StoreNo = 1,
                ProductNo = "P1",
                Quantity = 10,
                LastUpdated = DateTime.Now
            };
            _mockInventoryService
                .Setup(x => x.UpdateStoreInventoryAsync(inventory))
                .Returns(Task.CompletedTask);
            // Act
            await _mockInventoryService.Object.UpdateStoreInventoryAsync(inventory);
            // Assert
            _mockInventoryService.Verify(x => x.UpdateStoreInventoryAsync(inventory), Times.Once);
        }

        [Fact]
        public async Task UpdateDistributionCenterInventory()
        {
            // Arrange
            var inventory = new DistributionCenterInventory
            {
                ProductNo = "P1",
                Quantity = 100,
                LastUpdated = DateTime.Now
            };
            _mockInventoryService
                .Setup(x => x.UpdateDistributionCenterInventoryAsync(inventory))
                .Returns(Task.CompletedTask);
            // Act
            await _mockInventoryService.Object.UpdateDistributionCenterInventoryAsync(inventory);
            // Assert
            _mockInventoryService.Verify(x => x.UpdateDistributionCenterInventoryAsync(inventory), Times.Once);
        }
    }
}
