using Microsoft.EntityFrameworkCore;
using ReStockApi.Models;
using ReStockApi.Services.Store;

namespace ReStockApiTest
{
    public class StoreServiceTest : IDisposable
    {
        private readonly ReStockDbContext _context;
        private readonly StoreService _storeService;

        public StoreServiceTest()
        {
            // Create a new in-memory database for each test
            var options = new DbContextOptionsBuilder<ReStockDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ReStockDbContext(options);
            _storeService = new StoreService(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task CreateNewStore_ValidStore_Success()
        {
            // Arrange
            var store = new Store
            {
                Id = 1,
                No = 101,
                Name = "Test Store",
                Country = "Denmark",
                Address = "Test Street 123"
            };

            // Act
            await _storeService.CreateNewStore(store);

            // Assert
            var savedStore = await _context.Stores.FindAsync(store.Id);
            Assert.NotNull(savedStore);
            Assert.Equal(store.No, savedStore.No);
            Assert.Equal(store.Name, savedStore.Name);
            Assert.Equal(store.Country, savedStore.Country);
            Assert.Equal(store.Address, savedStore.Address);
        }

        [Fact]
        public async Task DeleteStore_ExistingStore_Success()
        {
            // Arrange
            var store = new Store
            {
                Id = 1,
                No = 101,
                Name = "Test Store",
                Country = "Denmark",
                Address = "Test Street 123"
            };
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();

            // Act
            await _storeService.DeleteStore(store.Id);

            // Assert
            var deletedStore = await _context.Stores.FindAsync(store.Id);
            Assert.Null(deletedStore);
        }

        [Fact]
        public async Task GetAllStores_ReturnsAllStores()
        {
            // Arrange
            var stores = new List<Store>
            {
                new Store
                {
                    Id = 1,
                    No = 101,
                    Name = "Store 1",
                    Country = "Denmark",
                    Address = "Address 1"
                },
                new Store
                {
                    Id = 2,
                    No = 102,
                    Name = "Store 2",
                    Country = "Sweden",
                    Address = "Address 2"
                }
            };
            await _context.Stores.AddRangeAsync(stores);
            await _context.SaveChangesAsync();

            // Act
            var result = await _storeService.GetAllStores();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, s => s.No == 101 && s.Name == "Store 1");
            Assert.Contains(result, s => s.No == 102 && s.Name == "Store 2");
        }

        [Fact]
        public async Task GetStore_ExistingStore_ReturnsStore()
        {
            // Arrange
            var store = new Store
            {
                Id = 1,
                No = 101,
                Name = "Test Store",
                Country = "Denmark",
                Address = "Test Street 123"
            };
            await _context.Stores.AddAsync(store);
            await _context.SaveChangesAsync();

            // Act
            var result = await _storeService.GetStore(101);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(101, result.No);
            Assert.Equal("Test Store", result.Name);
            Assert.Equal("Denmark", result.Country);
            Assert.Equal("Test Street 123", result.Address);
        }

        [Fact]
        public async Task GetStore_NonExistingStore_ReturnsNull()
        {
            // Act
            var result = await _storeService.GetStore(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateStore_ValidStore_Success()
        {
            // Arrange
            var store = new Store
            {
                Id = 1,
                No = 101,
                Name = "Original Name",
                Country = "Denmark",
                Address = "Original Address"
            };
            await _context.Stores.AddAsync(store);
            await _context.SaveChangesAsync();

            // Clear the change tracker
            _context.ChangeTracker.Clear();

            var updatedStore = new Store
            {
                Id = 1,
                No = 101,
                Name = "Updated Name",
                Country = "Sweden",
                Address = "Updated Address"
            };

            // Act
            await _storeService.UpdateStore(updatedStore);

            // Assert
            var result = await _context.Stores.FindAsync(store.Id);
            Assert.NotNull(result);
            Assert.Equal("Updated Name", result.Name);
            Assert.Equal("Sweden", result.Country);
            Assert.Equal("Updated Address", result.Address);
            Assert.Equal(101, result.No);
        }
    }
}
