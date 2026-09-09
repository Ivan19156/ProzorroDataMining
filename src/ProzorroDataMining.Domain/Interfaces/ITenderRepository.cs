namespace ProzorroDataMining.Domain.Interfaces;

using ProzorroDataMining.Domain.Entities;

public interface ITenderRepository : IRepository<Tender>
{
    Task<bool> ExistsAsync(string id, CancellationToken ct = default);
    Task UpsertAsync(Tender tender, CancellationToken ct = default);
}