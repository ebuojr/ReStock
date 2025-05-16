using System.Security.Cryptography;

namespace ReStockApi.Services.Reorder
{
    public interface IReorderService
    {
        /// <summary>
        /// Creates potential reorder orders for a given store based on inventory thresholds and distribution center availability.
        /// </summary>
        /// <param name="storeNo">The store number.</param>
        /// <returns>A list of reorder orders to be created for the store.</returns>
        Task<List<Models.Reorder>> CreatePotentialOrdersByStoreNoAsync(int storeNo);
    }
}
