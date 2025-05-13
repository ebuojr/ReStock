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
        }

        [Fact]
        public async Task CreateUpdateDeleteProduct_Works()
        {
            var product = new { ItemNo = "TEST999", Name = "Test Product", Brand = "BrandX", RetailPrice = 100.0, IsActive = true };
            var createResp = await _client.PostAsJsonAsync("/api/product/create", product);
            createResp.EnsureSuccessStatusCode();

            var updateProduct = new { ItemNo = "TEST999", Name = "Updated Product", Brand = "BrandX", RetailPrice = 102.0, IsActive = true };
            var updateResp = await _client.PutAsJsonAsync("/api/product/update", updateProduct);
            updateResp.EnsureSuccessStatusCode();

            var deleteResp = await _client.DeleteAsync("/api/product/delete/1"); // Adjust ID as needed
            deleteResp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetProductByNo_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/product/get/TEST999");
            response.EnsureSuccessStatusCode();
        }
    }
}
