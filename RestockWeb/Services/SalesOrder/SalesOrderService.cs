using RestockWeb.DTOs;
using RestockWeb.Models;
using System.Security.Cryptography;

namespace RestockWeb.Services.SalesOrder
{
    public class SalesOrderService : HttpService, ISalesOrderService
    {
        private const string BaseUrl = "api/salesorder";

        public SalesOrderService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task CreateSalesOrderAsync(List<Models.Reorder> reorders)
        {
            await PostAsync($"{BaseUrl}/craete-sales-order", reorders);
        }

        public async Task<List<SalesOrderDTO>> GetAllSalesOrdersAsync()
        {
            return await GetAsync<List<SalesOrderDTO>>($"{BaseUrl}/get-all") ?? new List<SalesOrderDTO>();
        }

        public async Task<SalesOrderDTO> GetSalesOrderByHeaderNoAsync(string headerNo)
        {
            return await GetAsync<SalesOrderDTO>($"{BaseUrl}/get-sales-order-by-headerNo/{headerNo}");
        }

        public async Task<List<SalesOrderDTO>> GetSalesOrderByStoreNoAsync(int storeNo)
        {
            return await GetAsync<List<SalesOrderDTO>>($"{BaseUrl}/get-sales-order-by-storeNo/{storeNo}") ?? new List<SalesOrderDTO>();
        }

        public async Task<Models.SalesOrder> UpdateSalesOrderHeaderAsync(Models.SalesOrder salesOrder)
        {
            await PutAsync($"{BaseUrl}", salesOrder);
            return salesOrder;
        }
    }
}