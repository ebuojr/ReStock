using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ReStockApi;
using Microsoft.VisualStudio.TestPlatform.TestHost;

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
            var fromdate = System.DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd");
            var response = await _client.GetAsync($"/api/reorderlog/get?fromdate={fromdate}&type=&no=&storeNo=");
            response.EnsureSuccessStatusCode();
        }
    }
}
