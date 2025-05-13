using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ReStockApi;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ReStockApi.Models;
using FluentAssertions;

namespace ReStockApiTest.IntegrationTest
{
    public class StoreControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public StoreControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllStores_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/store/all");
            response.EnsureSuccessStatusCode();
        }        [Fact]
        public async Task CreateUpdateDeleteStore_Works()
        {
            // Create a new store with the correct model fields (No instead of StoreNo)
            var store = new { No = 5555, Name = "Test Store", Country = "Test Country", Address = "Test Address" };
            var createResp = await _client.PostAsJsonAsync("/api/store/create", store);
            createResp.EnsureSuccessStatusCode();

            // Update the store with the correct model fields (No instead of StoreNo)
            var updateStore = new { No = 5555, Name = "Updated Store", Country = "Updated Country", Address = "Updated Address" };
            var updateResp = await _client.PutAsJsonAsync("/api/store/update", updateStore);
            updateResp.EnsureSuccessStatusCode();            // First, get the store to find its ID
            var getStoreResp = await _client.GetAsync("/api/store/get/5555");
            getStoreResp.EnsureSuccessStatusCode();
            var storeObj = await getStoreResp.Content.ReadFromJsonAsync<ReStockApi.Models.Store>();
            
            // Delete the store using its ID, not the No property
            var deleteResp = await _client.DeleteAsync($"/api/store/delete/{storeObj.Id}");
            deleteResp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetStoreByNo_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/store/get/5001");
            response.EnsureSuccessStatusCode();
        }
    }
}
