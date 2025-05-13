using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RestockWeb.Pages.Products;
using RestockWeb.Models;
using RestockWeb.Services.Product;
using Blazored.Toast.Services;

namespace ReStockApiTest.UserInterfaceTest
{
    public class ProductBunitTest : TestContext
    {
        [Fact]
        public async Task ProductOverviewComponent_FetchesAndUpdatesProducts_ShowsToast()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
            var mockToastService = new Mock<IToastService>();

            var products = new List<Product>
            {
                new Product { Id = 1, ItemNo = "A1", Name = "Test Product", Brand = "BrandX", RetailPrice = 100, IsActive = true },
                new Product { Id = 2, ItemNo = "B2", Name = "Another Product", Brand = "BrandY", RetailPrice = 200, IsActive = false }
            };

            mockProductService.Setup(s => s.GetProductsAsync()).ReturnsAsync(products);
            mockProductService.Setup(s => s.UpdateProductAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            Services.AddSingleton<IProductService>(mockProductService.Object);
            Services.AddSingleton<IToastService>(mockToastService.Object);

            // Act
            var cut = RenderComponent<ProductOverviewComponent>();

            // Assert fetch
            Assert.Contains("Test Product", cut.Markup);
            Assert.Contains("Another Product", cut.Markup);

            // Simulate update by making UpdateProduct public for test
            var updatedProduct = new Product
            {
                Id = 1,
                ItemNo = "A1",
                Name = "Updated Name",
                Brand = "BrandX",
                RetailPrice = 100,
                IsActive = true
            };
            mockProductService.Setup(s => s.GetProductsAsync()).ReturnsAsync(new List<Product> { updatedProduct, products[1] });

            // Use reflection to call the protected method
            var method = cut.Instance.GetType().GetMethod("UpdateProduct", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.NotNull(method); // Ensure method exists
            var task = (Task)method.Invoke(cut.Instance, new object[] { updatedProduct })!;
            await task;
            cut.Render();

            // Assert update in UI and toast
            Assert.Contains("Updated Name", cut.Markup);
            mockToastService.Verify(t => t.ShowSuccess(It.Is<string>(msg => msg.Contains("updated")), null), Times.Once);
            mockProductService.Verify(s => s.UpdateProductAsync(It.Is<Product>(p => p.Name == "Updated Name")), Times.Once);
        }
    }
}
