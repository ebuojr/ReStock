using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ReStockApi;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ReStockApiTest.IntegrationTest
{
    public class SalesOrderControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public SalesOrderControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllSalesOrders_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/salesorder/get-all");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetSalesOrderByHeaderNo_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/salesorder/get-sales-order-by-headerNo/1");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetSalesOrderByStoreNo_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/salesorder/get-sales-order-by-storeNo/1");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateUpdateSalesOrder_Works()
        {
            var reorder = new[] { new { StoreNo = 5090, ItemNo = "TEST999", Quantity = 1 } };
            var createResp = await _client.PostAsJsonAsync("/api/salesorder/craete-sales-order", reorder);
            createResp.EnsureSuccessStatusCode();

            var salesOrder = new { HeaderNo = "SO900", StoreNo = 5090 };
            var updateResp = await _client.PutAsJsonAsync("/api/salesorder", salesOrder);
            updateResp.EnsureSuccessStatusCode();
        }
    }
}
