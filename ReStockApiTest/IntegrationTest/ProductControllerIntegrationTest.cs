using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ReStockApi;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ReStockApiTest.IntegrationTest
{
    public class ProductControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProductControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllProducts_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/product/all");
            response.EnsureSuccessStatusCode();
        }        [Fact]
        public async Task CreateUpdateDeleteProduct_Works()
        {
            // Create a new product with the correct model fields and a RetailPrice within the valid range (69-1200)
            var product = new { Id = 0, ItemNo = "ZIZ-222-2222", Name = "Test Product", Brand = "BrandX", RetailPrice = 99.0, IsActive = true };
            var createResp = await _client.PostAsJsonAsync("/api/product/create", product);
            createResp.EnsureSuccessStatusCode();

            // Update the product with a valid RetailPrice
            var updateProduct = new { Id = 0, ItemNo = "ZIZ-222-2222", Name = "Updated Product", Brand = "BrandX", RetailPrice = 120.0, IsActive = true };
            var updateResp = await _client.PutAsJsonAsync("/api/product/update", updateProduct);
            updateResp.EnsureSuccessStatusCode();

            // Products are deleted by their integer Id, not the ItemNo string
            // Since we can't know the exact Id, we need to fetch it first
            var getResp = await _client.GetAsync($"/api/product/get/ZIZ-222-2222");
            getResp.EnsureSuccessStatusCode();
            var productObj = await getResp.Content.ReadFromJsonAsync<ReStockApi.Models.Product>();
            
            // Now delete using the actual Id
            var deleteResp = await _client.DeleteAsync($"/api/product/delete/{productObj.Id}");
            deleteResp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetProductByNo_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/product/get/ZIZ-111-1111");
            response.EnsureSuccessStatusCode();
        }
    }
}
