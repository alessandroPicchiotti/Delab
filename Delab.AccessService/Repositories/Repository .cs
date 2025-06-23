

using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;

namespace Delab.AccessService.Repositories;

public class Repository : IRepository 
{
    private readonly HttpClient _httpClient;
    private readonly Func<Task<string?>> _getToken;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public Repository(HttpClient httpClient, Func<Task<string?>> getToken)
    {
        _httpClient = httpClient;
        _getToken = getToken;
    }

    private async Task AddAuthorizationHeader()
    {
        // Rimuovi prima eventuali header esistenti per evitare duplicati
        _httpClient.DefaultRequestHeaders.Remove("Authorization");

        var token = await _getToken();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<HttpResponseWrapper<byte[]>> GetFileAsync(string url)
    {
        await AddAuthorizationHeader();
        var response = await _httpClient.GetAsync(url);

        return response.IsSuccessStatusCode
            ? new HttpResponseWrapper<byte[]>(await response.Content.ReadAsByteArrayAsync(), false, response)
            : new HttpResponseWrapper<byte[]>(Array.Empty<byte>(), true, response);
    }

    public async Task<HttpResponseWrapper<object?>> GetAsync(string url)
    {
        await AddAuthorizationHeader();
        var response = await _httpClient.GetAsync(url);
        return new HttpResponseWrapper<object?>(null, !response.IsSuccessStatusCode, response);
    }

    public async Task<HttpResponseWrapper<TResult?>> GetAsync<TResult>(string url) where TResult : class
    {
        await AddAuthorizationHeader();
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return new HttpResponseWrapper<TResult?>(default, true, response);

        try
        {
            var result = await response.Content.ReadFromJsonAsync<TResult>(_jsonOptions);
            return new HttpResponseWrapper<TResult?>(result, false, response);
        }
        catch (JsonException)
        {
            return new HttpResponseWrapper<TResult?>(default, true, response);
        }
    }

    public async Task<HttpResponseWrapper<object?>> PostAsync<TData>(string url, TData model) where TData : class
    {
        await AddAuthorizationHeader();
        var content = new StringContent(JsonSerializer.Serialize(model, _jsonOptions), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        return new HttpResponseWrapper<object?>(null, !response.IsSuccessStatusCode, response);
    }

    public async Task<HttpResponseWrapper<TResponse?>> PostAsync<TData, TResponse>(string url, TData model)
        where TData : class
        where TResponse : class
    {
        await AddAuthorizationHeader();
        var content = new StringContent(JsonSerializer.Serialize(model, _jsonOptions), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
            return new HttpResponseWrapper<TResponse?>(default, true, response);

        try
        {
            var result = await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
            return new HttpResponseWrapper<TResponse?>(result, false, response);
        }
        catch (JsonException)
        {
            return new HttpResponseWrapper<TResponse?>(default, true, response);
        }
    }

    public async Task<HttpResponseWrapper<object?>> PutAsync<TData>(string url, TData model) where TData : class
    {
        await AddAuthorizationHeader();
        var content = new StringContent(JsonSerializer.Serialize(model, _jsonOptions), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(url, content);
        return new HttpResponseWrapper<object?>(null, !response.IsSuccessStatusCode, response);
    }

    public async Task<HttpResponseWrapper<TResponse?>> PutAsync<TData, TResponse>(string url, TData model)
        where TData : class
        where TResponse : class
    {
        await AddAuthorizationHeader();
        var content = new StringContent(JsonSerializer.Serialize(model, _jsonOptions), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(url, content);

        if (!response.IsSuccessStatusCode)
            return new HttpResponseWrapper<TResponse?>(default, true, response);

        try
        {
            var result = await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
            return new HttpResponseWrapper<TResponse?>(result, false, response);
        }
        catch (JsonException)
        {
            return new HttpResponseWrapper<TResponse?>(default, true, response);
        }
    }

    public async Task<HttpResponseWrapper<object?>> DeleteAsync(string url)
    {
        await AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync(url);
        return new HttpResponseWrapper<object?>(null, !response.IsSuccessStatusCode, response);
    }

    public async Task<HttpResponseWrapper<TResponse?>> DeleteAsync<TResponse>(string url) where TResponse : class
    {
        await AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync(url);

        if (!response.IsSuccessStatusCode)
            return new HttpResponseWrapper<TResponse?>(default, true, response);

        try
        {
            var result = await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
            return new HttpResponseWrapper<TResponse?>(result, false, response);
        }
        catch (JsonException)
        {
            return new HttpResponseWrapper<TResponse?>(default, true, response);
        }
    }
}
