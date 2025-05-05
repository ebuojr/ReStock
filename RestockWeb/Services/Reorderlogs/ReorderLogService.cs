using RestockWeb.Models;

namespace RestockWeb.Services.Reorderlogs
{
    public class ReorderLogService : HttpService, IReorderLogService
    {
        private const string BaseUrl = "api/reorderlog";
        public ReorderLogService(HttpClient httpClient) : base(httpClient)
        {
        }
        public Task<IEnumerable<ReOrderLog>> GetLogsAsync(DateTime fromdate, string type, string no, string storeNo)
        {
            var url = $"{BaseUrl}/get?fromdate={fromdate:yyyy-MM-dd}&type={type}&no={no}&storeNo={storeNo}";
            return GetAsync<IEnumerable<ReOrderLog>>(url);
        }

        public Task LogAsync(int storeNo, string ItemNo, int quantity, string eventType, string description, bool ordered)
        {
            throw new NotImplementedException();
        }
    }
}
