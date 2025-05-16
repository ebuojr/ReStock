using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ReStockApi.Models;

namespace ReStockApi.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly ReStockDbContext _db;
        private readonly IValidator<Models.Product> _validator;

        /// <summary>
        /// Initializes a new instance of the ProductService class.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="validator">The product validator.</param>
        public ProductService(ReStockDbContext db, IValidator<Models.Product> validator)
        {
            _db = db;
            _validator = validator;
        }

        /// <summary>
        /// Creates a new product in the database after validation.
        /// </summary>
        /// <param name="product">The product to create.</param>
        public async Task CreateProductAsync(Models.Product product)
        {
            // validate
            var result = await _validator.ValidateAsync(product);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            await _db.Products.AddAsync(product);
            _db.SaveChanges();
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        public async Task DeleteProductAsync(int id)
        {
            var temp = await _db.Products.FindAsync(id);
            if (temp is null)
                throw new Exception("Product not found");

            _db.Products.Remove(temp);
            _db.SaveChanges();
        }

        /// <summary>
        /// Gets a product by its item number.
        /// </summary>
        /// <param name="ItemNo">The item number of the product.</param>
        /// <returns>The product if found; otherwise, null.</returns>
        public async Task<Models.Product> GetProductByNoAsync(string ItemNo)
            => await _db.Products.FirstOrDefaultAsync(p => p.ItemNo == ItemNo);

        /// <summary>
        /// Gets all products from the database.
        /// </summary>
        /// <returns>A list of all products.</returns>
        public async Task<List<Models.Product>> GetProductsAsync()
            => await _db.Products.ToListAsync();

        /// <summary>
        /// Updates an existing product after validation.
        /// </summary>
        /// <param name="product">The product to update.</param>
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
