using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ReStockApi;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ReStockApiTest.IntegrationTest
{
    public class InventoryControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public InventoryControllerIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetStoreInventoryByStoreNoWithThresholds_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/inventory/store-inventory-with-Threshold-store-no?storeNo=1");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetStoreInventoryByStoreNo_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/inventory/store?storeNo=1");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetStoreInventory_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/inventory/store-item?storeNo=1&ItemNo=TEST999");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UpdateStoreInventory_ReturnsOk()
        {
            var inventory = new { StoreNo = 1, ItemNo = "TEST999", Quantity = 10 };
            var response = await _client.PutAsJsonAsync("/api/inventory/update-store-inventory", inventory);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetDistributionCenterInventory_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/inventory/distribution-center-inventory");
            response.EnsureSuccessStatusCode();
        }
    }
}
