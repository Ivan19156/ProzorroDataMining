namespace ProzorroDataMining.Application.Services;

using Microsoft.Extensions.Logging;
using ProzorroDataMining.Application.Http;
using ProzorroDataMining.Application.Mappers;
using ProzorroDataMining.Contracts.DTOs.Prozorro;
using ProzorroDataMining.Domain.Interfaces;
using ProzorroDataMining.Shared.Constants;
using Tender = ProzorroDataMining.Domain.Entities.Tender;

public sealed class TenderIngestionService : ITenderIngestionService
{
    private readonly IProzorroClient _prozorroClient;
    private readonly ITenderRepository _repository;
    private readonly ILogger<TenderIngestionService> _logger;

    private readonly SemaphoreSlim _semaphore = new(5, 5);

    public TenderIngestionService(
        IProzorroClient prozorroClient,
        ITenderRepository repository,
        ILogger<TenderIngestionService> logger)
    {
        _prozorroClient = prozorroClient;
        _repository = repository;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        string? offset = null;
        var totalSaved = 0;

        _logger.LogInformation("ETL ingestion started");

        while (!ct.IsCancellationRequested)
        {
            var listResponse = await _prozorroClient
                .GetTenderListAsync(offset, ct);

            if (listResponse?.Data is null || listResponse.Data.Count == 0)
            {
                _logger.LogInformation("No more data from API");
                break;
            }

            
            if (listResponse.Data.Any(t => t.DateModified < ProzorroConstants.Filters.PeriodStart))
            {
                _logger.LogInformation("Reached period boundary, stopping pagination");
                break;
            }

            var inPeriod = listResponse.Data
                .Where(t => t.DateModified >= ProzorroConstants.Filters.PeriodStart
                         && t.DateModified < ProzorroConstants.Filters.PeriodEnd)
                .ToList();

            _logger.LogInformation(
                "Page fetched: {Total} items, {InPeriod} in target period",
                listResponse.Data.Count, inPeriod.Count);

            
            var tasks = inPeriod.Select(item => FetchAndFilterAsync(item, ct));
            var results = await Task.WhenAll(tasks);

           
            var validTenders = results.Where(t => t is not null).ToList();
            foreach (var tender in validTenders)
                await _repository.UpsertAsync(tender!, ct);

            totalSaved += validTenders.Count;

            offset = listResponse.NextPage?.Offset;
            if (offset is null) break;
        }

        _logger.LogInformation("ETL ingestion completed. Total saved: {Count}", totalSaved);
    }

    private async Task<Tender?> FetchAndFilterAsync(
        TenderListItem item,
        CancellationToken ct)
    {
        await _semaphore.WaitAsync(ct);
        try
        {
            var detail = await _prozorroClient.GetTenderDetailAsync(item.Id, ct);
            if (detail?.Data is null) return null;

            if (detail.Data.Status != ProzorroConstants.TargetStatus) return null;

            var hasCpv = detail.Data.Items?
                .Any(i => i.Classification?.Id == ProzorroConstants.TargetCpvCode)
                ?? false;

            if (!hasCpv) return null;

            return TenderMapper.MapToEntity(detail.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process tender {Id}", item.Id);
            return null;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}