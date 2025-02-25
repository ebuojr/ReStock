
using Dapper.FastCrud;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ReStockService.Product
{
    public class ProductService : IProductService
    {
        private string _connectionString;
        public ProductService(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection GetConnection() => new SqlConnection(_connectionString);

        public async Task CreateProductAsync(ReStockDomain.Product product)
            => await GetConnection().InsertAsync<ReStockDomain.Product>(product);

        public Task DeleteProductAsync(int id)
            => GetConnection().DeleteAsync(new ReStockDomain.Product { Id = id });

        public async Task<ReStockDomain.Product> GetProductByNoAsync(string productNo)
            => await GetConnection().GetAsync(new ReStockDomain.Product { No = productNo });

        public Task<IEnumerable<ReStockDomain.Product>> GetProductsAsync()
            => GetConnection().FindAsync<ReStockDomain.Product>();

        public Task UpdateProductAsync(ReStockDomain.Product product)
            => GetConnection().UpdateAsync<ReStockDomain.Product>(product);
    }
}
