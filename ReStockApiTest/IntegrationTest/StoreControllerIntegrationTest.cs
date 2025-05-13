using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ReStockApi;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ReStockApiTest.IntegrationTest
{
    public class StoreControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public StoreControllerIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllStores_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/store/all");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateUpdateDeleteStore_Works()
        {
            var store = new { StoreNo = 999, Name = "Test Store", Address = "Test Address" };
            var createResp = await _client.PostAsJsonAsync("/api/store/create", store);
            createResp.EnsureSuccessStatusCode();

            var updateStore = new { StoreNo = 999, Name = "Updated Store", Address = "Updated Address" };
            var updateResp = await _client.PutAsJsonAsync("/api/store/update", updateStore);
            updateResp.EnsureSuccessStatusCode();

            var deleteResp = await _client.DeleteAsync("/api/store/delete/999");
            deleteResp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetStoreByNo_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/store/get/1");
            response.EnsureSuccessStatusCode();
        }
    }
}
