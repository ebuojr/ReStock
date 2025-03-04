
namespace ReStockApi.Services.Product
{
    public interface IProductService
    {
        Task<Models.Product> GetProductByNoAsync(string ItemNo);
        Task<IEnumerable<Models.Product>> GetProductsAsync();
        Task CreateProductAsync(Models.Product product);
        Task UpdateProductAsync(Models.Product product);
        Task DeleteProductAsync(int id);
    }
}
