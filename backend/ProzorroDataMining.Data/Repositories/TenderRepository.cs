namespace ProzorroDataMining.Data.Repositories;

using Microsoft.EntityFrameworkCore;
using ProzorroDataMining.Domain.Entities;
using ProzorroDataMining.Domain.Interfaces;

public sealed class TenderRepository : BaseRepository<Tender>, ITenderRepository
{
    public TenderRepository(AppDbContext context) : base(context) { }

    public async Task<bool> ExistsAsync(string id, CancellationToken ct = default)
        => await DbSet.AnyAsync(t => t.Id == id, ct);

    public async Task UpsertAsync(Tender tender, CancellationToken ct = default)
    {
        var existing = await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == tender.Id, ct);

        if (existing is null)
        {
            await DbSet.AddAsync(tender, ct);
        }
        else if (existing.DateModified < tender.DateModified)
        {
            Context.Entry(tender).State = EntityState.Modified;
        }

        await Context.SaveChangesAsync(ct);
    }
}