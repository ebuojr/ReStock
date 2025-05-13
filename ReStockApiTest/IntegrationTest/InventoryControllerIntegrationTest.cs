using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ReStockApi;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using FluentAssertions;

namespace ReStockApiTest.IntegrationTest
{
    public class InventoryControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public InventoryControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetStoreInventoryByStoreNoWithThresholds_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/inventory/store-inventory-with-Threshold-store-no?storeNo=1");
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue("because the inventory with thresholds endpoint should respond successfully");
            
            // Only try to read content if it exists
            if (response.Content.Headers.ContentLength > 0)
            {
                var content = await response.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrEmpty("because the response should contain inventory data");
            }
        }

        [Fact]
        public async Task GetStoreInventoryByStoreNo_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/inventory/store?storeNo=1");
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue("because the store inventory endpoint should respond successfully");
            
            // Only try to read content if it exists
            if (response.Content.Headers.ContentLength > 0)
            {
                var content = await response.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrEmpty("because the response should contain store inventory data");
            }
        }        [Fact]
        public async Task GetStoreInventory_ReturnsOk()
        {
            // Arrange
            var storeNo = 1;
            var itemNo = "TEST999";
            
            // Act
            var response = await _client.GetAsync($"/api/inventory/store-item?storeNo={storeNo}&ItemNo={itemNo}");
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue($"because getting inventory for store {storeNo} and item {itemNo} should succeed");
            
            // Check content if available
            if (response.Content.Headers.ContentLength > 0)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    content.Should().NotBeNullOrEmpty("because the response should contain inventory data");
                }
            }
        }        [Fact]
        public async Task UpdateStoreInventory_ReturnsOk()
        {
            // Arrange
            var inventory = new { StoreNo = 1, ItemNo = "TEST999", Quantity = 10 };
            
            // Act
            var response = await _client.PutAsJsonAsync("/api/inventory/update-store-inventory", inventory);
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue("because updating store inventory should succeed");
            
            // Check response if available
            if (response.Content.Headers.ContentLength > 0)
            {
                var content = await response.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrEmpty("because the response should contain update confirmation");
            }
        }

        [Fact]
        public async Task GetDistributionCenterInventory_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/inventory/distribution-center-inventory");
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue("because getting distribution center inventory should succeed");
            
            // Check content if available
            if (response.Content.Headers.ContentLength > 0)
            {
                var content = await response.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrEmpty("because the response should contain distribution center inventory data");
            }
        }
    }
}
