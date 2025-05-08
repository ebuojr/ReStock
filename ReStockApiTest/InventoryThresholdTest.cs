using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ReStockApi.Models;
using ReStockApi.Services.Product;
using ReStockApi.Services.Store;
using ReStockApi.Services.Threshold;
using Xunit;

namespace ReStockApiTest
{
    public class InventoryThresholdTest : IDisposable
    {
        private readonly ReStockDbContext _context;
        private readonly Mock<IStoreService> _storeServiceMock;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<IValidator<InventoryThreshold>> _validatorMock;
        private readonly Mock<ILogger<ThresholdService>> _loggerMock;
        private readonly ThresholdService _service;

        public InventoryThresholdTest()
        {
            var options = new DbContextOptionsBuilder<ReStockDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ReStockDbContext(options);

            _storeServiceMock = new Mock<IStoreService>();
            _productServiceMock = new Mock<IProductService>();
            _validatorMock = new Mock<IValidator<InventoryThreshold>>();
            _loggerMock = new Mock<ILogger<ThresholdService>>();

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<InventoryThreshold>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _service = new ThresholdService(
                _context,
                _storeServiceMock.Object,
                _productServiceMock.Object,
                _validatorMock.Object,
                _loggerMock.Object
            );
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task CreateThreshold_Valid_AddsThreshold()
        {
            var threshold = new InventoryThreshold
            {
                StoreNo = 1,
                ItemNo = "ITEM1",
                MinimumQuantity = 1,
                TargetQuantity = 2,
                ReorderQuantity = 1,
                LastUpdated = DateTime.UtcNow
            };

            await _service.CreateThreshold(threshold);

            var result = await _context.InventoryThresholds.FirstOrDefaultAsync();
            result.Should().NotBeNull();
            result.StoreNo.Should().Be(1);
            result.ItemNo.Should().Be("ITEM1");
        }

        [Fact]
        public async Task CreateThreshold_Invalid_ThrowsValidationException()
        {
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<InventoryThreshold>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(
                    new[] { new FluentValidation.Results.ValidationFailure("StoreNo", "Invalid") }
                ));

            var threshold = new InventoryThreshold();

            Func<Task> act = async () => await _service.CreateThreshold(threshold);

            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task DeleteThresholdAsync_DeletesExisting()
        {
            var threshold = new InventoryThreshold
            {
                StoreNo = 2,
                ItemNo = "ITEM2",
                MinimumQuantity = 1,
                TargetQuantity = 2,
                ReorderQuantity = 1,
                LastUpdated = DateTime.UtcNow
            };
            _context.InventoryThresholds.Add(threshold);
            await _context.SaveChangesAsync();

            await _service.DeleteThresholdAsync(2, "ITEM2");

            var result = await _context.InventoryThresholds.FirstOrDefaultAsync(x => x.StoreNo == 2 && x.ItemNo == "ITEM2");
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteThresholdAsync_NonExisting_DoesNothing()
        {
            // Should not throw
            await _service.DeleteThresholdAsync(999, "NOTFOUND");
        }

        [Fact]
        public async Task GetThresholdAsync_ReturnsCorrectThreshold()
        {
            var threshold = new InventoryThreshold
            {
                StoreNo = 3,
                ItemNo = "ITEM3",
                MinimumQuantity = 1,
                TargetQuantity = 2,
                ReorderQuantity = 1,
                LastUpdated = DateTime.UtcNow
            };
            _context.InventoryThresholds.Add(threshold);
            await _context.SaveChangesAsync();

            var result = await _service.GetThresholdAsync(3, "ITEM3");
            result.Should().NotBeNull();
            result.StoreNo.Should().Be(3);
            result.ItemNo.Should().Be("ITEM3");
        }

        [Fact]
        public async Task GetThresholdAsync_NotFound_ReturnsNull()
        {
            var result = await _service.GetThresholdAsync(999, "NOTFOUND");
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetThresholdsAsync_ReturnsAll()
        {
            _context.InventoryThresholds.Add(new InventoryThreshold { StoreNo = 1, ItemNo = "A", MinimumQuantity = 1, TargetQuantity = 2, ReorderQuantity = 1, LastUpdated = DateTime.UtcNow });
            _context.InventoryThresholds.Add(new InventoryThreshold { StoreNo = 2, ItemNo = "B", MinimumQuantity = 1, TargetQuantity = 2, ReorderQuantity = 1, LastUpdated = DateTime.UtcNow });
            await _context.SaveChangesAsync();

            var result = await _service.GetThresholdsAsync();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetThresholdsByStoreNoAsync_ReturnsForStore()
        {
            _context.InventoryThresholds.Add(new InventoryThreshold { StoreNo = 5, ItemNo = "A", MinimumQuantity = 1, TargetQuantity = 2, ReorderQuantity = 1, LastUpdated = DateTime.UtcNow });
            _context.InventoryThresholds.Add(new InventoryThreshold { StoreNo = 5, ItemNo = "B", MinimumQuantity = 1, TargetQuantity = 2, ReorderQuantity = 1, LastUpdated = DateTime.UtcNow });
            _context.InventoryThresholds.Add(new InventoryThreshold { StoreNo = 6, ItemNo = "C", MinimumQuantity = 1, TargetQuantity = 2, ReorderQuantity = 1, LastUpdated = DateTime.UtcNow });
            await _context.SaveChangesAsync();

            var result = await _service.GetThresholdsByStoreNoAsync(5);
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task UpdateThresholdAsync_Valid_UpdatesThreshold()
        {
            var threshold = new InventoryThreshold
            {
                StoreNo = 7,
                ItemNo = "ITEM7",
                MinimumQuantity = 1,
                TargetQuantity = 2,
                ReorderQuantity = 1,
                LastUpdated = DateTime.UtcNow
            };
            _context.InventoryThresholds.Add(threshold);
            await _context.SaveChangesAsync();

            threshold.MinimumQuantity = 99;
            await _service.UpdateThresholdAsync(threshold);

            var updated = await _context.InventoryThresholds.FirstOrDefaultAsync(x => x.StoreNo == 7 && x.ItemNo == "ITEM7");
            updated.MinimumQuantity.Should().Be(99);
        }

        [Fact]
        public async Task UpdateThresholdAsync_Invalid_ThrowsValidationException()
        {
            var threshold = new InventoryThreshold
            {
                StoreNo = 8,
                ItemNo = "ITEM8",
                MinimumQuantity = 1,
                TargetQuantity = 2,
                ReorderQuantity = 1,
                LastUpdated = DateTime.UtcNow
            };
            _context.InventoryThresholds.Add(threshold);
            await _context.SaveChangesAsync();

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<InventoryThreshold>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(
                    new[] { new FluentValidation.Results.ValidationFailure("StoreNo", "Invalid") }
                ));

            threshold.MinimumQuantity = -1;
            Func<Task> act = async () => await _service.UpdateThresholdAsync(threshold);

            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
