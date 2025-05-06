using System.Net.Http.Json;
using System.Text.Json;

namespace RestockWeb.Services
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        protected async Task<T?> GetAsync<T>(string endpoint)
        {
            return await _httpClient.GetFromJsonAsync<T>(endpoint, _jsonOptions);
        }

        protected async Task<bool> PostAsync<T>(string endpoint, T data)
        {
            var content = JsonContent.Create(data);
            var response = await _httpClient.PostAsync(endpoint, content);
            return response.IsSuccessStatusCode;
        }

        protected async Task<T?> PostAsyncWithResponse<T, TRequest>(string endpoint, TRequest data)
        {
            var content = JsonContent.Create(data);
            var response = await _httpClient.PostAsync(endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
            }

            return default;
        }

        protected async Task<bool> PutAsync<T>(string endpoint, T data)
        {
            var content = JsonContent.Create(data);
            var response = await _httpClient.PutAsync(endpoint, content);
            return response.IsSuccessStatusCode;
        }

        protected async Task<bool> DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }
    }
}
