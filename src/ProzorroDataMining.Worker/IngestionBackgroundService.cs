namespace ProzorroDataMining.Worker;

using ProzorroDataMining.Application.Services;

public sealed class IngestionBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<IngestionBackgroundService> _logger;

    public IngestionBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<IngestionBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider
            .GetRequiredService<ITenderIngestionService>();

        try
        {
            await service.RunAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ingestion background service failed");
        }
    }
}