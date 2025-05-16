using ReStockApi.DTOs;
using ReStockApi.Models;
using System.Security.Cryptography;

namespace ReStockApi.Services.Inventory
{
    public interface IInventoryService
    {
        /// <summary>
        /// Gets all store inventory records with thresholds for a specific store.
        /// </summary>
        /// <param name="storeNo">The store number.</param>
        /// <returns>A list of inventory records with threshold information.</returns>
        Task<List<StoresInventoryWithThresholdDTO>> GetStoreInventoryByStoreNoWithThresholdsAsync(int storeNo);

        /// <summary>
        /// Gets all store inventory records for a specific store.
        /// </summary>
        /// <param name="storeNo">The store number.</param>
        /// <returns>A list of store inventory records.</returns>
        Task<List<StoreInventory>> GetStoreInventoryByStoreNoAsync(int storeNo);

        /// <summary>
        /// Gets the store inventory for a specific store and item.
        /// </summary>
        /// <param name="storeNo">The store number.</param>
        /// <param name="ItemNo">The item number.</param>
        /// <returns>The store inventory record.</returns>
        Task<StoreInventory> GetStoreInventoryAsync(int storeNo, string ItemNo);

        /// <summary>
        /// Inserts or updates a store inventory record after validation.
        /// </summary>
        /// <param name="inventory">The inventory record to upsert.</param>
        Task UpsertStoreInventoryAsync(StoreInventory inventory);

        /// <summary>
        /// Gets all distribution center inventory records.
        /// </summary>
        /// <returns>A list of all distribution center inventory records.</returns>
        Task<List<DistributionCenterInventory>> GetDistributionCenterInventoryAsync();

        /// <summary>
        /// Gets the distribution center inventory for a specific item.
        /// </summary>
        /// <param name="ItemNo">The item number.</param>
        /// <returns>The distribution center inventory record.</returns>
        Task<DistributionCenterInventory> GetDistributionCenterInventoryAsync(string ItemNo);

        /// <summary>
        /// Inserts or updates a distribution center inventory record.
        /// </summary>
        /// <param name="inventory">The inventory record to upsert.</param>
        Task UpsertDistributionCenterInventoryAsync(DistributionCenterInventory inventory);

        /// <summary>
        /// Checks the availability of a given item in the distribution center inventory.
        /// </summary>
        /// <param name="itemNo">The item number to check.</param>
        /// <param name="quantity">The requested quantity.</param>
        /// <returns>The available quantity (may be less than requested).</returns>
        Task<int> CheckAvailabilityAsync(string itemNo, int quantity);

        /// <summary>
        /// Increases the inventory quantity for a specific item in a store.
        /// </summary>
        /// <param name="storeNo">The store number.</param>
        /// <param name="itemNo">The item number.</param>
        /// <param name="quantity">The quantity to increase.</param>
        Task IncreaseStoreInventoryAsync(int storeNo, string itemNo, int quantity);

        /// <summary>
        /// Decreases the inventory quantity for a specific item in a store.
        /// </summary>
        /// <param name="storeNo">The store number.</param>
        /// <param name="itemNo">The item number.</param>
        /// <param name="quantity">The quantity to decrease.</param>
        Task DecreaseStoreInventoryAsync(int storeNo, string itemNo, int quantity);

        /// <summary>
        /// Decreases the inventory quantity for a specific item in the distribution center.
        /// </summary>
        /// <param name="itemNo">The item number.</param>
        /// <param name="quantity">The quantity to decrease.</param>
        Task DescreaseDistributionCenterInventoryAsync(string itemNo, int quantity);
    }
}
