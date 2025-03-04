using Microsoft.EntityFrameworkCore;
using ReStockDomain;

namespace ReStockService.Product
{
    public class ProductService : IProductService
    {
        private readonly ReStockDbContext _db;

        public ProductService(ReStockDbContext db)
        {
            _db = db;
        }

        public async Task CreateProductAsync(ReStockDomain.Product product)
        {
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var temp = await _db.Products.FindAsync(id);
            if (temp is null)
                throw new Exception("Product not found");

            _db.Products.Remove(temp);
            await _db.SaveChangesAsync();
        }

        public async Task<ReStockDomain.Product> GetProductByNoAsync(string ItemNo)
            => await _db.Products.FirstOrDefaultAsync(p => p.ItemNo == ItemNo);

        public async Task<IEnumerable<ReStockDomain.Product>> GetProductsAsync()
            => await _db.Products.ToListAsync();

        public async Task UpdateProductAsync(ReStockDomain.Product product)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }
    }
}
