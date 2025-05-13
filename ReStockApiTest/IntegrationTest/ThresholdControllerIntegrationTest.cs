using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ReStockApi;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ReStockApiTest.IntegrationTest
{
    public class ThresholdControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ThresholdControllerIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetThresholds_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/threshold/all");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetThreshold_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/threshold/store-item?storeNo=1&ItemNo=TEST999");
            response.EnsureSuccessStatusCode();
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
