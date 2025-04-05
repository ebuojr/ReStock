using RestockWeb.Models;

namespace RestockWeb.Services.Store
{
    public class StoreService : HttpService, IStoreService
    {
        private const string BaseUrl = "api/store";

        public StoreService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<bool> CreateStoreAsync(Models.Store store)
        {
            return await PostAsync($"{BaseUrl}/create", store);
        }

        public async Task<bool> DeleteStoreAsync(int id)
        {
            return await DeleteAsync($"{BaseUrl}/delete/{id}");
        }

        public async Task<List<Models.Store>?> GetAllStoresAsync()
        {
            return await GetAsync<List<Models.Store>>($"{BaseUrl}/all");
        }

        public async Task<Models.Store?> GetStoreAsync(int storeNo)
        {
            return await GetAsync<Models.Store>($"{BaseUrl}/get/{storeNo}");
        }

        public async Task<bool> UpdateStoreAsync(Models.Store store)
        {
            return await PutAsync($"{BaseUrl}/update", store);
        }
    }
}