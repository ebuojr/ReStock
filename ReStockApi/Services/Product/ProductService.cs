using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ReStockApi.Models;

namespace ReStockApi.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly ReStockDbContext _db;
        private readonly IValidator<Models.Product> _validator;

        public ProductService(ReStockDbContext db, IValidator<Models.Product> validator)
        {
            _db = db;
            _validator = validator;
        }

        public async Task CreateProductAsync(Models.Product product)
        {
            // validate
            var result = await _validator.ValidateAsync(product);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            await _db.Products.AddAsync(product);
            _db.SaveChanges();
        }

        public async Task DeleteProductAsync(int id)
        {
            var temp = await _db.Products.FindAsync(id);
            if (temp is null)
                throw new Exception("Product not found");

            _db.Products.Remove(temp);
            _db.SaveChanges();
        }

        public async Task<Models.Product> GetProductByNoAsync(string ItemNo)
            => await _db.Products.FirstOrDefaultAsync(p => p.ItemNo == ItemNo);

        public async Task<List<Models.Product>> GetProductsAsync()
            => await _db.Products.ToListAsync();

        public async Task UpdateProductAsync(Models.Product product)
        {
            // validate
            var result = await _validator.ValidateAsync(product);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            _db.Products.Update(product);
            _db.SaveChanges();
        }
    }
}
