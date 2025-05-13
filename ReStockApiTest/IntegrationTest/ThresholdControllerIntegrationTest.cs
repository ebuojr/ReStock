using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ReStockApi;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using FluentAssertions;
using ReStockApi.Models;

namespace ReStockApiTest.IntegrationTest
{
    public class ThresholdControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ThresholdControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetThresholds_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/threshold/all");
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue("because retrieving all thresholds should succeed");
            
            // Check content if available
            if (response.Content.Headers.ContentLength > 0)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var thresholds = await response.Content.ReadFromJsonAsync<List<InventoryThreshold>>();
                    thresholds.Should().NotBeNull("because the API should return a list of thresholds (even if empty)");
                }
            }
        }

        [Fact]
        public async Task GetThreshold_ReturnsOk()
        {
            // Arrange
            var storeNo = 1;
            var itemNo = "TEST999";
            
            // Act
            var response = await _client.GetAsync($"/api/threshold/store-item?storeNo={storeNo}&ItemNo={itemNo}");
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue($"because retrieving threshold for store {storeNo} and item {itemNo} should succeed");
            
            // Check content if available
            if (response.Content.Headers.ContentLength > 0)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content) && content != "null")
                {
                    var threshold = await response.Content.ReadFromJsonAsync<InventoryThreshold>();
                    threshold.Should().NotBeNull("because we expect a threshold configuration to be returned");
                    
                    if (threshold != null)
                    {
                        threshold.StoreNo.Should().Be(storeNo, "because we requested this specific store number");
                        threshold.ItemNo.Should().Be(itemNo, "because we requested this specific item number");
                    }
                }
            }
        }

        [Fact]
        public async Task GetThresholdsByStoreNo_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/threshold/store?storeNo=1");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateUpdateThreshold_Works()
        {
            var threshold = new { StoreNo = 1, ItemNo = "TEST999", Threshold = 5 };
            var createResp = await _client.PostAsJsonAsync("/api/threshold", threshold);
            createResp.EnsureSuccessStatusCode();

            var updateThreshold = new { StoreNo = 1, ItemNo = "TEST999", Threshold = 10 };
            var updateResp = await _client.PutAsJsonAsync("/api/threshold/update", updateThreshold);
            updateResp.EnsureSuccessStatusCode();
        }
    }
}
