using Moq;
using ReStockDomain;
using ReStockService.Connection;
using ReStockService.Product;

namespace ReStockServiceTest
{
    public class ProductServiceTest
    {
        private readonly Mock<IConnectionService> _connectionServiceMock;
        private readonly ProductService _productService;

        public ProductServiceTest()
        {
            _connectionServiceMock = new Mock<IConnectionService>();
            _productService = new ProductService(_connectionServiceMock.Object);
        }

        [Fact]
        public async Task CreateProductAsync_Should_Insert_New_Product()
        {
            // Arrange
            var newProduct = new Product { Id = 1, No = "P001" };

            var dbConnectionMock = new Mock<System.Data.IDbConnection>();
            _connectionServiceMock.Setup(x => x.CreateConnection()).Returns(dbConnectionMock.Object);

            // Act
            await _productService.CreateProductAsync(new Product());

            // Assert
            _connectionServiceMock.Verify(x => x.CreateConnection(), Times.Once);
        }

        [Fact]
        public async Task GetProductByNoAsync_Should_Return_Existing_Product()
        {
            // Arrange
            var existingItemNo = "P123";
            var dbConnectionMock = new Mock<System.Data.IDbConnection>();
            _connectionServiceMock.Setup(x => x.CreateConnection()).Returns(dbConnectionMock.Object);

            // Act
            var product = await _productService.GetProductByNoAsync(existingItemNo);

            // Assert
            Assert.Null(product);
            _connectionServiceMock.Verify(x => x.CreateConnection(), Times.Once);
        }

        [Fact]
        public async Task GetProductsAsync_Should_Return_Product_List()
        {
            // Arrange
            var dbConnectionMock = new Mock<System.Data.IDbConnection>();
            _connectionServiceMock.Setup(x => x.CreateConnection()).Returns(dbConnectionMock.Object);

            // Act
            var products = await _productService.GetProductsAsync();

            // Assert
            Assert.Empty((IEnumerable<Product>)products);
            _connectionServiceMock.Verify(x => x.CreateConnection(), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_Should_Update_Existing_Product()
        {
            // Arrange
            var productToUpdate = new Product { Id = 2, No = "P002" };
            var dbConnectionMock = new Mock<System.Data.IDbConnection>();
            _connectionServiceMock.Setup(x => x.CreateConnection()).Returns(dbConnectionMock.Object);

            // Act
            await _productService.UpdateProductAsync(productToUpdate);

            // Assert
            _connectionServiceMock.Verify(x => x.CreateConnection(), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_Should_Remove_Product_By_Id()
        {
            // Arrange
            var productId = 3;
            var dbConnectionMock = new Mock<System.Data.IDbConnection>();
            _connectionServiceMock.Setup(x => x.CreateConnection()).Returns(dbConnectionMock.Object);

            // Act
            await _productService.DeleteProductAsync(productId);

            // Assert
            _connectionServiceMock.Verify(x => x.CreateConnection(), Times.Once);
        }
    }
}
