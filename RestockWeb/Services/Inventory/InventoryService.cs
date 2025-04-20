using RestockWeb.Models;

namespace RestockWeb.Services.Inventory
{
    public class InventoryService : HttpService, IInventoryService
    {
        private const string BaseUrl = "api/inventory";

        public InventoryService(HttpClient httpClient) : base(httpClient)
        {
        }

        public Task<List<DistributionCenterInventory>> GetDistributionCenterInventories()
        {
            return GetAsync<List<DistributionCenterInventory>>($"{BaseUrl}/distribution-center-inventory");
        }

        public async Task<DistributionCenterInventory?> GetDistributionCenterInventoryByItemNo(string itemNo)
        {
            return await GetAsync<DistributionCenterInventory>($"{BaseUrl}/distribution-center-inventory-by-item/{itemNo}");
        }

        public async Task<StoreInventory?> GetStoreInventoryAsync(int storeNo, string itemNo)
        {
            return await GetAsync<StoreInventory>($"{BaseUrl}/store/{storeNo}/{itemNo}");
        }

        public async Task<List<StoreInventory>?> GetStoreInventoryByStoreNoAsync(int storeNo)
        {
            return await GetAsync<List<StoreInventory>>($"{BaseUrl}/store-inventory-by-store-no?storeNo={storeNo}");
        }

        public async Task<bool> UpdateDistributionCenterInventoryAsync(DistributionCenterInventory inventory)
        {
            return await PutAsync($"{BaseUrl}/distribution-center-inventory", inventory);
        }

        public async Task UpdateStoreInventoryAsync(StoreInventory inventory)
        {
            await PutAsync($"{BaseUrl}/update-store-inventory", inventory);
        }
    }
}