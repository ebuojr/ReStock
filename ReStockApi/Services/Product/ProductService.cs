using Microsoft.EntityFrameworkCore;
using ReStockApi.Models;

namespace ReStockApi.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly ReStockDbContext _db;

        public ProductService(ReStockDbContext db)
        {
            _db = db;
        }

        public async Task CreateProductAsync(Models.Product product)
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

        public async Task<Models.Product> GetProductByNoAsync(string ItemNo)
            => await _db.Products.FirstOrDefaultAsync(p => p.ItemNo == ItemNo);

        public async Task<IEnumerable<Models.Product>> GetProductsAsync()
            => await _db.Products.ToListAsync();

        public async Task UpdateProductAsync(Models.Product product)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }
    }
}
