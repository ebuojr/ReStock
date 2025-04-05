using System.Net.Http.Json;
using System.Text;
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
            try
            {
                return await _httpClient.GetFromJsonAsync<T>(endpoint, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAsync: {ex.Message}");
                return default;
            }
        }

        protected async Task<bool> PostAsync<T>(string endpoint, T data)
        {
            try
            {
                var content = JsonContent.Create(data);
                var response = await _httpClient.PostAsync(endpoint, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PostAsync: {ex.Message}");
                return false;
            }
        }

        protected async Task<T?> PostAsyncWithResponse<T, TRequest>(string endpoint, TRequest data)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PostAsyncWithResponse: {ex.Message}");
                return default;
            }
        }

        protected async Task<bool> PutAsync<T>(string endpoint, T data)
        {
            try
            {
                var content = JsonContent.Create(data);
                var response = await _httpClient.PutAsync(endpoint, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PutAsync: {ex.Message}");
                return false;
            }
        }

        protected async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                return false;
            }
        }
    }
}