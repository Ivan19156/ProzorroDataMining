namespace ProzorroDataMining.Application.Services;

using Microsoft.EntityFrameworkCore;
using ProzorroDataMining.Contracts.Analytics;
using ProzorroDataMining.Data;

public sealed class AnalyticsService : IAnalyticsService
{
    private readonly AppDbContext _context;

    public AnalyticsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BudgetSavingsResponse> GetBudgetSavingsAsync(
        CancellationToken ct = default)
    {
        var totalBudget = await _context.Tenders
            .AsNoTracking()
            .SumAsync(t => t.BudgetAmount, ct);

        var totalContracts = await _context.TenderContracts
            .AsNoTracking()
            .Where(c => c.ContractValue.HasValue)
            .SumAsync(c => c.ContractValue!.Value, ct);

        return new BudgetSavingsResponse
        {
            TotalBudget = totalBudget,
            TotalContracts = totalContracts,
            TotalSavings = totalBudget - totalContracts
        };
    }

    public async Task<IEnumerable<TopBuyerResponse>> GetTopBuyersAsync(
        int top = 5,
        CancellationToken ct = default)
    {
        return await _context.Tenders
            .AsNoTracking()
            .Join(_context.TenderContracts,
                t => t.Id,
                c => c.TenderId,
                (t, c) => new { t.ProcuringEntityName, c.ContractValue })
            .Where(x => x.ContractValue.HasValue)
            .GroupBy(x => x.ProcuringEntityName)
            .Select(g => new TopBuyerResponse
            {
                Name = g.Key,
                TotalAmount = g.Sum(x => x.ContractValue!.Value),
                ContractCount = g.Count()
            })
            .OrderByDescending(x => x.TotalAmount)
            .Take(top)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<TopSupplierResponse>> GetTopSuppliersAsync(
        int top = 5,
        CancellationToken ct = default)
    {
        return await _context.TenderAwards
            .AsNoTracking()
            .Where(a => a.AwardValue.HasValue)
            .GroupBy(a => a.SupplierName)
            .Select(g => new TopSupplierResponse
            {
                Name = g.Key,
                TotalAmount = g.Sum(a => a.AwardValue!.Value),
                AwardCount = g.Count()
            })
            .OrderByDescending(x => x.TotalAmount)
            .Take(top)
            .ToListAsync(ct);
    }
}