using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ReStockApi.Models;
using ReStockApi.Services.Product;
using ReStockApi.Validation;

namespace ReStockApiTest
{
    public class ProductServiceTest : IDisposable
    {
        private readonly ReStockDbContext _context;
        private readonly IValidator<Product> _validator;
        private readonly ProductService _productService;

        public ProductServiceTest()
        {
            // Create a new in-memory database for each test
            var options = new DbContextOptionsBuilder<ReStockDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ReStockDbContext(options);
            _validator = new ProductdValidator();
            _productService = new ProductService(_context, _validator);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task CreateProductAsync_ValidProduct_Success()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                ItemNo = "ITEM001",
                Name = "Test Product",
                Brand = "Zizzi",
                RetailPrice = 99.99m,
                IsActive = true
            };

            // Act
            await _productService.CreateProductAsync(product);

            // Assert
            var savedProduct = await _context.Products.FindAsync(product.Id);
            Assert.NotNull(savedProduct);
            Assert.Equal(product.ItemNo, savedProduct.ItemNo);
            Assert.Equal(product.Name, savedProduct.Name);
            Assert.Equal(product.Brand, savedProduct.Brand);
            Assert.Equal(product.RetailPrice, savedProduct.RetailPrice);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-99.99)]
        public async Task CreateProductAsync_InvalidRetailPrice_ThrowsValidationException(decimal price)
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                ItemNo = "ITEM001",
                Name = "Test Product",
                Brand = "Zizzi",
                RetailPrice = price,
                IsActive = true
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(
                () => _productService.CreateProductAsync(product));

            Assert.Contains("RetailPrice must be greater than 0", exception.Message);
        }

        [Fact]
        public async Task CreateProductAsync_MissingRequiredFields_ThrowsValidationException()
        {
            // Arrange
            var product = new Product();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(
                () => _productService.CreateProductAsync(product));

            Assert.Contains("Name is required", exception.Message);
            Assert.Contains("ItemNo is required", exception.Message);
            Assert.Contains("Brand is required", exception.Message);
            Assert.Contains("RetailPrice must be greater than 0", exception.Message);
        }

        [Fact]
        public async Task DeleteProductAsync_ExistingProduct_Success()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                ItemNo = "ITEM001",
                Name = "Test Product",
                Brand = "Zizzi",
                RetailPrice = 99.99m,
                IsActive = true
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            await _productService.DeleteProductAsync(product.Id);

            // Assert
            var deletedProduct = await _context.Products.FindAsync(product.Id);
            Assert.Null(deletedProduct);
        }

        [Fact]
        public async Task DeleteProductAsync_NonExistingProduct_ThrowsException()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _productService.DeleteProductAsync(1));

            Assert.Equal("Product not found", exception.Message);
        }

        [Fact]
        public async Task GetProductByNoAsync_ExistingProduct_ReturnsProduct()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                ItemNo = "ITEM001",
                Name = "Test Product",
                Brand = "Zizzi",
                RetailPrice = 99.99m,
                IsActive = true
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _productService.GetProductByNoAsync("ITEM001");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ITEM001", result.ItemNo);
            Assert.Equal("Test Product", result.Name);
            Assert.Equal("Zizzi", result.Brand);
            Assert.Equal(99.99m, result.RetailPrice);
        }

        [Fact]
        public async Task GetProductByNoAsync_NonExistingProduct_ReturnsNull()
        {
            // Act
            var result = await _productService.GetProductByNoAsync("NONEXISTENT");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductsAsync_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    ItemNo = "ITEM001",
                    Name = "Product 1",
                    Brand = "Zizzi",
                    RetailPrice = 99.99m,
                    IsActive = true
                },
                new Product
                {
                    Id = 2,
                    ItemNo = "ITEM002",
                    Name = "Product 2",
                    Brand = "Zizzi",
                    RetailPrice = 149.99m,
                    IsActive = true
                }
            };
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            // Act
            var result = await _productService.GetProductsAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.ItemNo == "ITEM001" && p.RetailPrice == 99.99m);
            Assert.Contains(result, p => p.ItemNo == "ITEM002" && p.RetailPrice == 149.99m);
        }

        [Fact]
        public async Task UpdateProductAsync_ValidProduct_Success()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                ItemNo = "ITEM001",
                Name = "Original Name",
                Brand = "Zizzi",
                RetailPrice = 100m,
                IsActive = true
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Clear the change tracker
            _context.ChangeTracker.Clear();

            var updatedProduct = new Product
            {
                Id = 1,
                ItemNo = "ITEM001",
                Name = "Updated Name",
                Brand = "Zizzi",
                RetailPrice = 150m,
                IsActive = true
            };

            // Act
            await _productService.UpdateProductAsync(updatedProduct);

            // Assert
            var result = await _context.Products.FindAsync(product.Id);
            Assert.NotNull(result);
            Assert.Equal("Updated Name", result.Name);
            Assert.Equal(150m, result.RetailPrice);
            Assert.Equal("ITEM001", result.ItemNo);
            Assert.Equal("Zizzi", result.Brand);
        }

        [Fact]
        public async Task UpdateProductAsync_InvalidProduct_ThrowsValidationException()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                ItemNo = "",  // Invalid: Required field
                Name = "",    // Invalid: Required field
                Brand = "",   // Invalid: Required field
                RetailPrice = 0m,  // Invalid: Must be greater than 0
                IsActive = true
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(
                () => _productService.UpdateProductAsync(product));

            Assert.Contains("Name is required", exception.Message);
            Assert.Contains("ItemNo is required", exception.Message);
            Assert.Contains("Brand is required", exception.Message);
            Assert.Contains("RetailPrice must be greater than 0", exception.Message);
        }
    }
}
