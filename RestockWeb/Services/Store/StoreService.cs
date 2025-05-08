using RestockWeb.Models;

namespace RestockWeb.Services.Store
{
    public class StoreService : HttpService, IStoreService
    {
        private const string BaseUrl = "api/store";

        public StoreService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task CreateStoreAsync(Models.Store store)
        {
            await PostAsync($"{BaseUrl}/create", store);
        }

        public async Task DeleteStoreAsync(int id)
        {
            await DeleteAsync($"{BaseUrl}/delete/{id}");
        }

        public async Task<List<Models.Store>?> GetAllStoresAsync()
        {
            return await GetAsync<List<Models.Store>>($"{BaseUrl}/all");
        }

        public async Task<Models.Store?> GetStoreAsync(int storeNo)
        {
            return await GetAsync<Models.Store>($"{BaseUrl}/get/{storeNo}");
        }

        public async Task UpdateStoreAsync(Models.Store store)
        {
            await PutAsync($"{BaseUrl}/update", store);
        }
    }
}