using RestockWeb.Models;

namespace RestockWeb.Services.Inventory
{
    public class InventoryService : HttpService, IInventoryService
    {
        private const string BaseUrl = "api/inventory";

        public InventoryService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<DistributionCenterInventory?> GetDistributionCenterInventoryAsync(string itemNo)
        {
            return await GetAsync<DistributionCenterInventory>($"{BaseUrl}/dc/{itemNo}");
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
            return await PutAsync($"{BaseUrl}/dc/update", inventory);
        }

        public async Task<bool> UpdateStoreInventoryAsync(StoreInventory inventory)
        {
            return await PutAsync($"{BaseUrl}/store/update", inventory);
        }
    }
}