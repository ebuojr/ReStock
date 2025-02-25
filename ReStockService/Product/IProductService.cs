
namespace ReStockService.Product
{
    public interface IProductService
    {
        Task<ReStockDomain.Product> GetProductByNoAsync(string productNo);
        Task<IEnumerable<ReStockDomain.Product>> GetProductsAsync();
        Task CreateProductAsync(ReStockDomain.Product product);
        Task UpdateProductAsync(ReStockDomain.Product product);
        Task DeleteProductAsync(int id);
    }
}
