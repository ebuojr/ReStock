using RestockWeb.Models;

namespace RestockWeb.Services.Product
{
    public class ProductService : HttpService, IProductService
    {
        private const string BaseUrl = "api/product";

        public ProductService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<bool> CreateProductAsync(Models.Product product)
        {
            return await PostAsync($"{BaseUrl}/create", product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await DeleteAsync($"{BaseUrl}/delete/{id}");
        }

        public async Task<Models.Product?> GetProductByNoAsync(string itemNo)
        {
            return await GetAsync<Models.Product>($"{BaseUrl}/get/{itemNo}");
        }

        public async Task<List<Models.Product>?> GetProductsAsync()
        {
            return await GetAsync<List<Models.Product>>($"{BaseUrl}/all");
        }

        public async Task<bool> UpdateProductAsync(Models.Product product)
        {
            return await PutAsync($"{BaseUrl}/update", product);
        }
    }
}