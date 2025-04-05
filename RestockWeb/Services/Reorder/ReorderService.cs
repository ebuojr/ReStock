using RestockWeb.Models;

namespace RestockWeb.Services.Reorder
{
    public class ReorderService : HttpService, IReorderService
    {
        private const string BaseUrl = "api/reorder";
        
        public ReorderService(HttpClient httpClient) : base(httpClient)
        {
        }
        
        public async Task<bool> CreateReorderAsync(Models.Reorder reorder)
        {
            return await PostAsync($"{BaseUrl}/create", reorder);
        }
        
        public async Task<Models.Reorder?> GetReorderAsync(int id)
        {
            return await GetAsync<Models.Reorder>($"{BaseUrl}/get/{id}");
        }
        
        public async Task<List<Models.Reorder>?> GetReordersAsync()
        {
            return await GetAsync<List<Models.Reorder>>($"{BaseUrl}/all");
        }
        
        public async Task<bool> ProcessReorderAsync(int id)
        {
            return await PostAsync($"{BaseUrl}/process/{id}", new { });
        }
        
        public async Task<List<ReOrderLog>?> GetReorderLogsAsync()
        {
            return await GetAsync<List<ReOrderLog>>($"{BaseUrl}/logs");
        }
    }
}