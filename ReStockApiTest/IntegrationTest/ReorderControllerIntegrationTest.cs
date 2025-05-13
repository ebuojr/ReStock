using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ReStockApi;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ReStockApiTest.IntegrationTest
{
    public class ReorderControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ReorderControllerIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreatePotentialOrders_ReturnsOk()
        {
            var response = await _client.PostAsJsonAsync("/api/reorder/create-potential-orders", 1);
            response.EnsureSuccessStatusCode();
        }
    }
}
