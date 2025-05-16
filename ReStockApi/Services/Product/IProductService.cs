namespace ReStockApi.Services.Product
{
    public interface IProductService
    {
        /// <summary>
        /// Gets a product by its item number.
        /// </summary>
        /// <param name="ItemNo">The item number of the product.</param>
        /// <returns>The product if found; otherwise, null.</returns>
        Task<Models.Product> GetProductByNoAsync(string ItemNo);

        /// <summary>
        /// Gets all products from the database.
        /// </summary>
        /// <returns>A list of all products.</returns>
        Task<List<Models.Product>> GetProductsAsync();

        /// <summary>
        /// Creates a new product in the database after validation.
        /// </summary>
        /// <param name="product">The product to create.</param>
        Task CreateProductAsync(Models.Product product);

        /// <summary>
        /// Updates an existing product after validation.
        /// </summary>
        /// <param name="product">The product to update.</param>
        Task UpdateProductAsync(Models.Product product);

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        Task DeleteProductAsync(int id);
    }
}
