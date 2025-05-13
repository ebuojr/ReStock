using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ReStockApi;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using FluentAssertions;
using ReStockApi.Models;

namespace ReStockApiTest.IntegrationTest
{
    public class ReorderlogControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ReorderlogControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetLogs_ReturnsOk()
        {
            // Arrange
            var fromdate = System.DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd");
            
            // Act
            var response = await _client.GetAsync($"/api/reorderlog/get?fromdate={fromdate}&type=&no=&storeNo=");
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue("because retrieving reorder logs should succeed");
            
            // Check content if available
            if (response.Content.Headers.ContentLength > 0)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var logs = await response.Content.ReadFromJsonAsync<List<ReOrderLog>>();
                    logs.Should().NotBeNull("because the API should return a list of logs (even if empty)");
                }
            }
        }
    }
}
