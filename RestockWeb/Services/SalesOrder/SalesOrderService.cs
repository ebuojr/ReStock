using RestockWeb.Models;

namespace RestockWeb.Services.SalesOrder
{
    public class SalesOrderService : HttpService, ISalesOrderService
    {
        private const string BaseUrl = "api/salesorder";

        public SalesOrderService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task CreateSalesOrderAsync(Models.SalesOrder order, List<SalesOrderLine> orderLines)
        {
            var createOrderDto = new
            {
                Order = order,
                OrderLines = orderLines
            };
            
            await PostAsync($"{BaseUrl}/create", createOrderDto);
        }

        public async Task<Models.SalesOrder?> GetSalesOrderAsync(string headerNo)
        {
            return await GetAsync<Models.SalesOrder>($"{BaseUrl}/get/{headerNo}");
        }

        public async Task<List<SalesOrderLine>?> GetSalesOrderLinesAsync(string headerNo)
        {
            return await GetAsync<List<SalesOrderLine>>($"{BaseUrl}/lines/{headerNo}");
        }

        public async Task<List<Models.SalesOrder>?> GetSalesOrdersAsync()
        {
            return await GetAsync<List<Models.SalesOrder>>($"{BaseUrl}/all");
        }

        public async Task UpdateSalesOrderStatusAsync(string headerNo, OrderStatus status)
        {
            await PutAsync($"{BaseUrl}/update-status/{headerNo}", new { Status = status });
        }
    }
}