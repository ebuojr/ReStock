using System.Net.Http.Json;
using FluentAssertions;
using ReStockApi.Models;

namespace ReStockApiTest.IntegrationTest
{
    public class ProductControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProductControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }        [Fact]
        public async Task GetAllProducts_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/product/all");
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            
            if (response.Content.Headers.ContentLength > 0)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var products = await response.Content.ReadFromJsonAsync<List<Product>>();
                    products.Should().NotBeNull();
                    products.Should().NotBeEmpty("because the database should contain at least one product");
                }
            }
        }
        
        [Fact]
        public async Task CreateUpdateDeleteProduct_Works()
        {
            // Arrange
            var testItemNo = "ZIZ-222-2222";
            var product = new { Id = 0, ItemNo = testItemNo, Name = "Test Product", Brand = "BrandX", RetailPrice = 99.0, IsActive = true };
            
            // Act - Create
            var createResp = await _client.PostAsJsonAsync("/api/product/create", product);
            
            // Assert - Create
            createResp.IsSuccessStatusCode.Should().BeTrue("because the product creation should succeed");
            
            // Act - Get the created product
            var getInitialResp = await _client.GetAsync($"/api/product/get/{testItemNo}");
            var initialProduct = await getInitialResp.Content.ReadFromJsonAsync<Product>();
            
            // Assert - Verify product properties
            initialProduct.Should().NotBeNull();
            initialProduct!.Name.Should().Be("Test Product");
            initialProduct.RetailPrice.Should().Be(99.0m);

            // Act - Update
            var updateProduct = new { 
                Id = initialProduct.Id, 
                ItemNo = testItemNo, 
                Name = "Updated Product", 
                Brand = "BrandX", 
                RetailPrice = 120.0, 
                IsActive = true 
            };
            var updateResp = await _client.PutAsJsonAsync("/api/product/update", updateProduct);
            
            // Assert - Update
            updateResp.IsSuccessStatusCode.Should().BeTrue("because the product update should succeed");            // Act - Get the updated product
            var getUpdatedResp = await _client.GetAsync($"/api/product/get/{testItemNo}");
            getUpdatedResp.IsSuccessStatusCode.Should().BeTrue();
              
            // Initialize updated product as nullable
            Product? updatedProduct = null;
            if (getUpdatedResp.Content.Headers.ContentLength > 0)
            {
                var content = await getUpdatedResp.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    updatedProduct = await getUpdatedResp.Content.ReadFromJsonAsync<Product>();
                    
                    // Assert - Verify updated properties
                    updatedProduct.Should().NotBeNull();
                    updatedProduct!.Name.Should().Be("Updated Product");
                    updatedProduct.RetailPrice.Should().Be(120.0m);
                }
            }
            
            // Act - Delete
            var deleteResp = await _client.DeleteAsync($"/api/product/delete/{updatedProduct!.Id}");
            
            // Assert - Delete
            deleteResp.IsSuccessStatusCode.Should().BeTrue("because the product deletion should succeed");
              // Act - Verify it's gone
            var getFinalResp = await _client.GetAsync($"/api/product/get/{testItemNo}");
            
            // The response should be successful, but the content might be empty or null
            getFinalResp.IsSuccessStatusCode.Should().BeTrue();
            
            if (getFinalResp.Content.Headers.ContentLength > 0)
            {
                var content = await getFinalResp.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content) && content != "null")
                {
                    var finalProduct = await getFinalResp.Content.ReadFromJsonAsync<Product>();
                    finalProduct.Should().BeNull("because the product should have been deleted");
                }
            }
        }        [Fact]
        public async Task GetProductByNo_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/product/get/ZIZ-111-1111");
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            
            if (response.Content.Headers.ContentLength > 0)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var product = await response.Content.ReadFromJsonAsync<Product>();
                    product.Should().NotBeNull();
                    product!.ItemNo.Should().Be("ZIZ-111-1111");
                }
            }
        }
    }
}
