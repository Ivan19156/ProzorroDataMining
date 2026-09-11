namespace ProzorroDataMining.Application.Http;

using Microsoft.Extensions.Logging;
using ProzorroDataMining.Contracts.DTOs.Prozorro;
using ProzorroDataMining.Shared.Constants;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

public sealed class ProzorroClient : IProzorroClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProzorroClient> _logger;

    public ProzorroClient(HttpClient httpClient, ILogger<ProzorroClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<TenderListResponse?> GetTenderListAsync(
        string? offset = null,
        CancellationToken ct = default)
    {
        var url = ProzorroConstants.Api.TenderListFiltered;
        if (offset is not null)
            url += $"&offset={offset}";

        return await GetAsync<TenderListResponse>(url, ct);
    }

    public async Task<TenderDetailResponse?> GetTenderDetailAsync(
        string id,
        CancellationToken ct = default)
        => await GetAsync<TenderDetailResponse>(string.Format(ProzorroConstants.Api.TenderDetail(id)), ct);

    private async Task<T?> GetAsync<T>(string url, CancellationToken ct)
    {
        try
        {
            var response = await _httpClient.GetAsync(url, ct);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>(
                cancellationToken: ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error fetching {Url}", url);
            return default;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON parse error for {Url}", url);
            return default;
        }
    }
}