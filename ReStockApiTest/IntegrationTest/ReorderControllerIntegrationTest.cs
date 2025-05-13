using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ReStockApi;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using FluentAssertions;

namespace ReStockApiTest.IntegrationTest
{
    public class ReorderControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ReorderControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreatePotentialOrders_ReturnsOk()
        {
            // Arrange
            var storeNo = 5090;
            
            // Act
            var response = await _client.PostAsJsonAsync("/api/reorder/create-potential-orders", storeNo);
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue($"because creating potential orders for store {storeNo} should succeed");
            
            // Verify response content if needed
            if (response.Content.Headers.ContentLength > 0)
            {
                var content = await response.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrEmpty("because the response should contain information about created orders");
            }
        }
    }
}
