namespace ProzorroDataMining.Application.Http;

using ProzorroDataMining.Contracts.DTOs.Prozorro;

public interface IProzorroClient
{
    Task<TenderListResponse?> GetTenderListAsync(
        string? offset = null,
        CancellationToken ct = default);

    Task<TenderDetailResponse?> GetTenderDetailAsync(
        string id,
        CancellationToken ct = default);
}