using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ReStockApi;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using FluentAssertions;
using ReStockApi.DTOs;

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
            // Act
            var response = await _client.GetAsync("/api/salesorder/get-all");
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue("because retrieving all sales orders should succeed");
            
            // Check content if available
            if (response.Content.Headers.ContentLength > 0)
            {
                var content = await response.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrEmpty("because the response should contain sales order data");
                
                if (!string.IsNullOrEmpty(content))
                {
                    var salesOrders = await response.Content.ReadFromJsonAsync<List<SalesOrderDTO>>();
                    salesOrders.Should().NotBeNull("because the API should return a list of sales orders (even if empty)");
                }
            }
        }

        [Fact]
        public async Task GetSalesOrderByHeaderNo_ReturnsOk()
        {
            // Arrange
            var headerNo = 1;
            
            // Act
            var response = await _client.GetAsync($"/api/salesorder/get-sales-order-by-headerNo/{headerNo}");
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue($"because retrieving sales order with header no {headerNo} should succeed");
            
            // Check content if available
            if (response.Content.Headers.ContentLength > 0)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content) && content != "null")
                {
                    var salesOrder = await response.Content.ReadFromJsonAsync<SalesOrderDTO>();
                    salesOrder.Should().NotBeNull($"because the sales order with header no {headerNo} should exist");
                }
            }
        }

        [Fact]
        public async Task GetSalesOrderByStoreNo_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/salesorder/get-sales-order-by-storeNo/5001");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateUpdateSalesOrder_Works()
        {
            var reorder = new[] { new { StoreNo = 5090, ItemNo = "ZIZ-111-1111", Quantity = 1 } };
            var createResp = await _client.PostAsJsonAsync("/api/salesorder/craete-sales-order", reorder);
            createResp.EnsureSuccessStatusCode();

            var salesOrder = new { HeaderNo = "SO900", StoreNo = 5090 };
            var updateResp = await _client.PutAsJsonAsync("/api/salesorder", salesOrder);
            updateResp.EnsureSuccessStatusCode();
        }
    }
}
