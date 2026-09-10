namespace ProzorroDataMining.Application.Services;

public interface ITenderIngestionService
{
    Task RunAsync(CancellationToken ct = default);
}