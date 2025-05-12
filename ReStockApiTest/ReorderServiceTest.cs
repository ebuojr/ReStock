using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ReStockApi.DTOs;
using ReStockApi.Models;
using ReStockApi.Services.Inventory;
using ReStockApi.Services.Reorder;
using ReStockApi.Services.ReorderLog;
using ReStockApi.Services.Store;

namespace ReStockApiTest
{
    public class ReorderServiceTest : IDisposable
    {
        private readonly Mock<IInventoryService> _inventoryServiceMock;
        private readonly Mock<IReorderLogService> _reorderLogServiceMock;
        private readonly Mock<IStoreService> _storeServiceMock;
        private readonly ReorderService _reorderService;
        private readonly ReStockDbContext _context;


        public ReorderServiceTest()
        {
            var options = new DbContextOptionsBuilder<ReStockDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ReStockDbContext(options);

            _inventoryServiceMock = new Mock<IInventoryService>();
            _reorderLogServiceMock = new Mock<IReorderLogService>();
            _storeServiceMock = new Mock<IStoreService>();

            _reorderService = new ReorderService(
                _context,
                _inventoryServiceMock.Object,
                _reorderLogServiceMock.Object,
                _storeServiceMock.Object
            );
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        public static IEnumerable<object[]> InvalidStoreNumbers =>
            new List<object[]>
            {
                new object[] { 0 },
                new object[] { -1 },
                new object[] { -99 }
            };

        [Theory]
        [MemberData(nameof(InvalidStoreNumbers))]
        public async Task CreatePotentialOrdersByStoreNoAsync_InvalidStoreNo_ThrowsArgumentNullException(int storeNo)
        {
            // Arrange
            _storeServiceMock.Setup(s => s.StoreExists(storeNo));

            // Act
            Func<Task> act = async () => await _reorderService.CreatePotentialOrdersByStoreNoAsync(storeNo);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("*Store number does not exist.*");
        }

        public static IEnumerable<object[]> NoThresholdsCases =>
            new List<object[]>
            {
                new object[] { null },
                new object[] { new List<StoresInventoryWithThresholdDTO>() }
            };

        [Theory]
        [MemberData(nameof(NoThresholdsCases))]
        public async Task CreatePotentialOrdersByStoreNoAsync_NoThresholds_ThrowsArgumentNullException(List<StoresInventoryWithThresholdDTO> thresholds)
        {
            // Arrange
            _storeServiceMock.Setup(s => s.GetStore(It.IsAny<int>())).ReturnsAsync(new Store());
            _inventoryServiceMock.Setup(i => i.GetStoreInventoryByStoreNoWithThresholdsAsync(It.IsAny<int>()))
                .ReturnsAsync(thresholds);

            // Act
            Func<Task> act = async () => await _reorderService.CreatePotentialOrdersByStoreNoAsync(123);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("*No thresholds found for the given store number.*");
        }

        public static IEnumerable<object[]> ThresholdsForReorderCases =>
            new List<object[]>
            {
                new object[]
                {
                    new List<StoresInventoryWithThresholdDTO>
                    {
                        new StoresInventoryWithThresholdDTO
                        {
                            ItemNo = "ITEM001",
                            CurrentQuantity = 5,
                            MinimumQuantity = 10,
                            TargetQuantity = 20,
                            ReorderQuantity = 10
                        }
                    },
                    5, // DC Inventory
                    ReorderLogType.DCInventory.ToString(),
                    false // ShouldAddToResult
                },
                new object[]
                {
                    new List<StoresInventoryWithThresholdDTO>
                    {
                        new StoresInventoryWithThresholdDTO
                        {
                            ItemNo = "ITEM002",
                            CurrentQuantity = 5,
                            MinimumQuantity = 10,
                            TargetQuantity = 20,
                            ReorderQuantity = 10
                        }
                    },
                    20, // DC Inventory
                    null,
                    true // ShouldAddToResult
                }
            };

        [Theory]
        [MemberData(nameof(ThresholdsForReorderCases))]
        public async Task CreatePotentialOrdersByStoreNoAsync_Thresholds_ReorderBehavior(List<StoresInventoryWithThresholdDTO> thresholds, int dcInventory, string? expectedLogType, bool shouldAddToResult)
        {
            // Arrange
            _storeServiceMock.Setup(s => s.GetStore(It.IsAny<int>())).ReturnsAsync(new Store());
            _inventoryServiceMock.Setup(i => i.GetStoreInventoryByStoreNoWithThresholdsAsync(It.IsAny<int>()))
                .ReturnsAsync(thresholds);
            _inventoryServiceMock.Setup(i => i.GetDistributionCenterInventoryAsync(It.IsAny<string>()))
                .ReturnsAsync(new DistributionCenterInventory { ItemNo = thresholds[0].ItemNo, Quantity = dcInventory });

            // Act
            var result = await _reorderService.CreatePotentialOrdersByStoreNoAsync(123);

            // Assert
            if (shouldAddToResult)
            {
                result.Should().HaveCount(1);
                result[0].StoreNo.Should().Be(123);
                result[0].ItemNo.Should().Be(thresholds[0].ItemNo);
            }
            else
            {
                result.Should().BeEmpty();
                if (expectedLogType != null)
                {
                    _reorderLogServiceMock.Verify(r => r.LogAsync(
                        123,
                        thresholds[0].ItemNo,
                        It.IsAny<int>(),
                        expectedLogType,
                        It.IsAny<string>(),
                        false
                    ), Times.Once);
                }
            }
        }
    }
}
