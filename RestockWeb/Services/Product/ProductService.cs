using RestockWeb.Models;

namespace RestockWeb.Services.Product
{
    public class ProductService : HttpService, IProductService
    {
        private const string BaseUrl = "api/product";

        public ProductService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task CreateProductAsync(Models.Product product)
        {
            await PostAsync($"{BaseUrl}/create", product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await DeleteAsync($"{BaseUrl}/delete/{id}");
        }

        public async Task<Models.Product?> GetProductByNoAsync(string itemNo)
        {
            return await GetAsync<Models.Product>($"{BaseUrl}/get/{itemNo}");
        }

        public async Task<List<Models.Product>?> GetProductsAsync()
        {
            return await GetAsync<List<Models.Product>>($"{BaseUrl}/all");
        }

        public async Task UpdateProductAsync(Models.Product product)
        {
            await PutAsync($"{BaseUrl}/update", product);
        }
    }
}