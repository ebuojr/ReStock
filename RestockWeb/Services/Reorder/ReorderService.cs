using RestockWeb.Models;

namespace RestockWeb.Services.Reorder
{
    public class ReorderService : HttpService, IReorderService
    {
        private const string BaseUrl = "api/reorder";
        
        public ReorderService(HttpClient httpClient) : base(httpClient)
        {
        }
        
        public async Task<List<Models.Reorder>> CreatePotentialOrdersByStoreNoAsync(int storeNo)
        {
            return await PostAsyncWithResponse<List<Models.Reorder>, int>($"{BaseUrl}/create-potential-orders", storeNo);
        }
    }
}