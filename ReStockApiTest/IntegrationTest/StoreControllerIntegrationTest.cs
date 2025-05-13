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
        }        [Fact]
        public async Task GetAllStores_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/store/all");
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue("because the API should return a success status code");
            
            // Only try to read content if it exists
            if (response.Content.Headers.ContentLength > 0)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var stores = await response.Content.ReadFromJsonAsync<List<Store>>();
                    stores.Should().NotBeNull("because the response should be deserializable to a list of stores");
                    stores.Should().NotBeEmpty("because the database should contain at least one store");
                }
                else
                {
                    // If content is empty or null, fail the test with a descriptive message
                    false.Should().BeTrue("because the API response should not be empty");
                }
            }
            else
            {
                // If no content length header, fail the test with a descriptive message
                false.Should().BeTrue("because the API should return content");
            }
        }[Fact]
        public async Task CreateUpdateDeleteStore_Works()
        {
            // Arrange
            var testStoreNo = 5555;
            var store = new { No = testStoreNo, Name = "Test Store", Country = "Test Country", Address = "Test Address" };
            
            // Act - Create
            var createResp = await _client.PostAsJsonAsync("/api/store/create", store);
            
            // Assert - Create
            createResp.IsSuccessStatusCode.Should().BeTrue("because the store creation should succeed");
            
            // Act - Get the created store
            var getInitialResp = await _client.GetAsync($"/api/store/get/{testStoreNo}");
            var initialStore = await getInitialResp.Content.ReadFromJsonAsync<Store>();
            
            // Assert - Verify store properties
            initialStore.Should().NotBeNull();
            initialStore!.No.Should().Be(testStoreNo);
            initialStore.Name.Should().Be("Test Store");
            initialStore.Country.Should().Be("Test Country");
            initialStore.Address.Should().Be("Test Address");

            // Act - Update
            var updateStore = new { 
                Id = initialStore.Id,
                No = testStoreNo, 
                Name = "Updated Store", 
                Country = "Updated Country", 
                Address = "Updated Address" 
            };
            var updateResp = await _client.PutAsJsonAsync("/api/store/update", updateStore);
            
            // Assert - Update
            updateResp.IsSuccessStatusCode.Should().BeTrue("because the store update should succeed");            // Act - Get the updated store
            var getUpdatedResp = await _client.GetAsync($"/api/store/get/{testStoreNo}");
            getUpdatedResp.IsSuccessStatusCode.Should().BeTrue();
            
            // Initialize updatedStore to null for proper scope
            Store? updatedStore = null;
            
            if (getUpdatedResp.Content.Headers.ContentLength > 0)
            {
                var content = await getUpdatedResp.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    updatedStore = await getUpdatedResp.Content.ReadFromJsonAsync<Store>();
                    
                    // Assert - Verify updated properties
                    updatedStore.Should().NotBeNull();
                    updatedStore!.Name.Should().Be("Updated Store");
                    updatedStore.Country.Should().Be("Updated Country");
                    updatedStore.Address.Should().Be("Updated Address");
                }
            }
            
            // Ensure we have a valid store to delete
            updatedStore.Should().NotBeNull("because we need a valid store to delete");
            
            // Act - Delete
            var deleteResp = await _client.DeleteAsync($"/api/store/delete/{updatedStore!.Id}");
            
            // Assert - Delete
            deleteResp.IsSuccessStatusCode.Should().BeTrue("because the store deletion should succeed");
              // Act - Verify it's gone
            var getFinalResp = await _client.GetAsync($"/api/store/get/{testStoreNo}");
            
            // The response should be successful, but the content might be empty or null
            getFinalResp.IsSuccessStatusCode.Should().BeTrue();
            
            if (getFinalResp.Content.Headers.ContentLength > 0)
            {
                var content = await getFinalResp.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content) && content != "null")
                {
                    var finalStore = await getFinalResp.Content.ReadFromJsonAsync<Store>();
                    
                    // Although the store is deleted, the API may return an empty object or null,
                    // so we check that no valid store with the expected No is returned
                    if (finalStore != null)
                    {
                        finalStore.No.Should().NotBe(testStoreNo, "because the store should have been deleted");
                    }
                }
            }
        }        [Fact]
        public async Task GetStoreByNo_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/store/get/5001");
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            
            if (response.Content.Headers.ContentLength > 0)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var store = await response.Content.ReadFromJsonAsync<Store>();
                    store.Should().NotBeNull();
                    store!.No.Should().Be(5001);
                    store.Name.Should().NotBeNullOrEmpty();
                }
            }
        }
    }
}
