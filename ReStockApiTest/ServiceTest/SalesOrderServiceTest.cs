using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ReStockApi.Models;
using ReStockApi.Services.Inventory;
using ReStockApi.Services.SalesOrder;

namespace ReStockApiTest.ServiceTest;

public class SalesOrderServiceTest : IDisposable
{
    private readonly Mock<IInventoryService> _inventoryServiceMock;
    private readonly SalesOrderService _salesOrderService;
    private readonly ReStockDbContext _context;

    public SalesOrderServiceTest()
    {
        var options = new DbContextOptionsBuilder<ReStockDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ReStockDbContext(options);
        _inventoryServiceMock = new Mock<IInventoryService>();
        _salesOrderService = new SalesOrderService(_context, _inventoryServiceMock.Object);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    public static IEnumerable<object[]> ValidReorderData =>
        new List<object[]>
        {
            new object[] { new Reorder { StoreNo = 1, ItemNo = "ITEM001", Quantity = 5, CreatedAt = DateTime.UtcNow } }
        };

    public static IEnumerable<object[]> InvalidReorderData =>
        new List<object[]>
        {
            // No SalesOrderNumber in DB
            new object[] { new Reorder { StoreNo = 1, ItemNo = "ITEM001", Quantity = 5, CreatedAt = DateTime.UtcNow } }
        };

    public static IEnumerable<object[]> SalesOrderByHeaderData =>
        new List<object[]>
        {
            new object[] { "SO-101", new SalesOrder { HeaderNo = "SO-101", StoreNo = 1, OrderDate = DateTime.UtcNow, OrderStatus = OrderStatus.Processing }, new SalesOrderLine { HeaderNo = "SO-101", LineNo = 100, ItemNo = "ITEM001", Quantity = 5 } },
            new object[] { "NON-EXISTENT", default(SalesOrder)!, default(SalesOrderLine)! }
        };

    public static IEnumerable<object[]> SalesOrderByStoreData =>
        new List<object[]>
        {
            new object[] { 2, new SalesOrder { HeaderNo = "SO-200", StoreNo = 2, OrderDate = DateTime.UtcNow, OrderStatus = OrderStatus.Processing }, new SalesOrderLine { HeaderNo = "SO-200", LineNo = 100, ItemNo = "ITEM002", Quantity = 10 } },
            new object[] { 999, default(SalesOrder)!, default(SalesOrderLine)! }
        };

    [Theory]
    [MemberData(nameof(ValidReorderData))]
    public async Task CreateSalesOrderAsync_ValidInput_CreatesOrder(Reorder reorder)
    {
        // Arrange
        _inventoryServiceMock.Setup(x => x.CheckAvailabilityAsync(reorder.ItemNo, reorder.Quantity)).ReturnsAsync(reorder.Quantity);
        _inventoryServiceMock.Setup(x => x.IncreaseStoreInventoryAsync(reorder.StoreNo, reorder.ItemNo, reorder.Quantity)).Returns(Task.CompletedTask);
        _inventoryServiceMock.Setup(x => x.DescreaseDistributionCenterInventoryAsync(reorder.ItemNo, reorder.Quantity)).Returns(Task.CompletedTask);
        _context.SalesOrderNumber.Add(new SalesOrderNumber { No = 100 });
        await _context.SaveChangesAsync();

        // Act
        await _salesOrderService.CreateSalesOrderAsync(new List<Reorder> { reorder });

        // Assert
        var orders = await _context.SalesOrders.ToListAsync();
        orders.Should().HaveCount(1);
        var lines = await _context.SalesOrderLines.ToListAsync();
        lines.Should().HaveCount(1);
        lines[0].ItemNo.Should().Be(reorder.ItemNo);
        lines[0].Quantity.Should().Be(reorder.Quantity);
    }

    [Theory]
    [MemberData(nameof(InvalidReorderData))]
    public async Task CreateSalesOrderAsync_InvalidInput_ThrowsException(Reorder reorder)
    {
        // Arrange: No SalesOrderNumber in DB
        _inventoryServiceMock.Setup(x => x.CheckAvailabilityAsync(reorder.ItemNo, reorder.Quantity)).ReturnsAsync(reorder.Quantity);
        _inventoryServiceMock.Setup(x => x.IncreaseStoreInventoryAsync(reorder.StoreNo, reorder.ItemNo, reorder.Quantity)).Returns(Task.CompletedTask);
        _inventoryServiceMock.Setup(x => x.DescreaseDistributionCenterInventoryAsync(reorder.ItemNo, reorder.Quantity)).Returns(Task.CompletedTask);

        // Act
        Func<Task> act = async () => await _salesOrderService.CreateSalesOrderAsync(new List<Reorder> { reorder });

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [MemberData(nameof(SalesOrderByHeaderData))]
    public async Task GetSalesOrderByHeaderNoAsync_VariousCases(string headerNo, SalesOrder order, SalesOrderLine line)
    {
        // Arrange
        if (!(order is null) && !(line is null))
        {
            _context.SalesOrders.Add(order);
            _context.SalesOrderLines.Add(line);
            await _context.SaveChangesAsync();
        }

        // Act
        var result = await _salesOrderService.GetSalesOrderByHeaderNoAsync(headerNo);

        // Assert
        if (!(order is null) && !(line is null))
        {
            result.Should().NotBeNull();
            result.SalesOrder.Should().NotBeNull();
            result.SalesOrder.HeaderNo.Should().Be(headerNo);
            result.SalesOrderLines.Should().ContainSingle(l => l.ItemNo == line.ItemNo && l.Quantity == line.Quantity);
        }
        else
        {
            result.Should().NotBeNull();
            result.SalesOrder.Should().BeNull();
            result.SalesOrderLines.Should().BeEmpty();
        }
    }

    [Theory]
    [MemberData(nameof(SalesOrderByStoreData))]
    public async Task GetSalesOrderByStoreNoAsync_VariousCases(int storeNo, SalesOrder order, SalesOrderLine line)
    {
        // Arrange
        if (!(order is null) && !(line is null))
        {
            _context.SalesOrders.Add(order);
            _context.SalesOrderLines.Add(line);
            await _context.SaveChangesAsync();
        }

        // Act
        var result = await _salesOrderService.GetSalesOrderByStoreNoAsync(storeNo);

        // Assert
        if (!(order is null) && !(line is null))
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].SalesOrder.StoreNo.Should().Be(storeNo);
            result[0].SalesOrderLines.Should().ContainSingle(l => l.ItemNo == line.ItemNo && l.Quantity == line.Quantity);
        }
        else
        {
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
