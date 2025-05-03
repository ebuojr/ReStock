using RestockWeb.Models;

namespace RestockWeb.Services.Threshold
{
    public class ThresholdService : HttpService, IThresholdService
    {
        private const string BaseUrl = "api/threshold";

        public ThresholdService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<bool> CreateThresholdAsync(InventoryThreshold threshold)
        {
            return await PostAsync($"{BaseUrl}/create", threshold);
        }

        public async Task<bool> DeleteThresholdAsync(int id)
        {
            return await DeleteAsync($"{BaseUrl}/delete/{id}");
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

        public async Task<bool> UpdateThresholdAsync(InventoryThreshold threshold)
        {
            return await PutAsync($"{BaseUrl}/update", threshold);
        }
    }
}