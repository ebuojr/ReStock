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

        protected async Task PostAsync<T>(string endpoint, T data)
        {
            var content = JsonContent.Create(data);
            var response = await _httpClient.PostAsync(endpoint, content);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"{await response.Content.ReadAsStringAsync()}", ex);
            }
        }

        protected async Task<T?> PostAsyncWithResponse<T, TRequest>(string endpoint, TRequest data)
        {
            var content = JsonContent.Create(data);
            var response = await _httpClient.PostAsync(endpoint, content);

            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), _jsonOptions);
            else
                throw new Exception($"Error: {await response.Content.ReadAsStringAsync()}");
        }

        protected async Task PutAsync<T>(string endpoint, T data)
        {
            var content = JsonContent.Create(data);
            var response = await _httpClient.PutAsync(endpoint, content);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"{await response.Content.ReadAsStringAsync()}", ex);
            }
        }

        protected async Task DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"{await response.Content.ReadAsStringAsync()}", ex);
            }
        }
    }
}
