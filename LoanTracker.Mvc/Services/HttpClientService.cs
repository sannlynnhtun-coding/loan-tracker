using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using LoanTracker.Shared.Models;

namespace LoanTracker.Mvc.Services;

public class HttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _authHeaderValue;

    public HttpClientService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["ApiBaseUrl"] ?? throw new Exception("ApiBaseUrl was not found.");

        var username = "admin"; // Should come from config in real app
        var password = "password";
        var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        _authHeaderValue = $"Basic {authString}";
    }

    private void AddAuthHeader()
    {
        _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(_authHeaderValue);
    }

    public async Task<Result<T>?> GetAsync<T>(string endpoint)
    {
        try
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync($"{_baseUrl}{endpoint}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Result<T>>(content);
        }
        catch (Exception ex)
        {
            return Result<T>.Failure($"Request failed: {ex.Message}");
        }
    }

    public async Task<Result<TResponse>?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        try
        {
            AddAuthHeader();
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}{endpoint}", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Result<TResponse>>(responseContent);
        }
        catch (Exception ex)
        {
            return Result<TResponse>.Failure($"Request failed: {ex.Message}");
        }
    }

    public async Task<Result<TResponse>?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        try
        {
            AddAuthHeader();
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}{endpoint}", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Result<TResponse>>(responseContent);
        }
        catch (Exception ex)
        {
            return Result<TResponse>.Failure($"Request failed: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteAsync(string endpoint)
    {
        try
        {
            AddAuthHeader();
            var response = await _httpClient.DeleteAsync($"{_baseUrl}{endpoint}");
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result<bool>>(responseContent);
            return result ?? Result<bool>.Failure("Failed to deserialize response.");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Request failed: {ex.Message}");
        }
    }
}
