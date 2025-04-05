using RestockWeb.Models;

namespace RestockWeb.Services.Product
{
    public interface IProductService
    {
        Task<Models.Product?> GetProductByNoAsync(string itemNo);
        Task<List<Models.Product>?> GetProductsAsync();
        Task<bool> CreateProductAsync(Models.Product product);
        Task<bool> UpdateProductAsync(Models.Product product);
        Task<bool> DeleteProductAsync(int id);
    }
}