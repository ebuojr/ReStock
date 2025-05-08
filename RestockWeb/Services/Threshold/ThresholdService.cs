using RestockWeb.Models;

namespace RestockWeb.Services.Threshold
{
    public class ThresholdService : HttpService, IThresholdService
    {
        private const string BaseUrl = "api/threshold";

        public ThresholdService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task CreateThresholdAsync(InventoryThreshold threshold)
        {
            await PostAsync($"{BaseUrl}/create", threshold);
        }

        public async Task DeleteThresholdAsync(int id)
        {
            await DeleteAsync($"{BaseUrl}/delete/{id}");
        }

        public async Task<List<InventoryThreshold>?> GetAllThresholdsAsync()
        {
            return await GetAsync<List<InventoryThreshold>>($"{BaseUrl}/all");
        }

        public async Task<InventoryThreshold?> GetThresholdAsync(int storeNo, string itemNo)
        {
            return await GetAsync<InventoryThreshold>($"{BaseUrl}/store-item?storeNo={storeNo}&ItemNo={itemNo}");
        }

        public async Task<List<InventoryThreshold>?> GetThresholdsByStoreAsync(int storeNo)
        {
            return await GetAsync<List<InventoryThreshold>>($"{BaseUrl}/store/{storeNo}");
        }

        public async Task UpdateThresholdAsync(InventoryThreshold threshold)
        {
            await PutAsync($"{BaseUrl}/update", threshold);
        }
    }
}