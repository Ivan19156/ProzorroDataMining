namespace ProzorroDataMining.Application.Services;

using ProzorroDataMining.Contracts.Analytics;

public interface IAnalyticsService
{
    Task<BudgetSavingsResponse> GetBudgetSavingsAsync(CancellationToken ct = default);
    Task<IEnumerable<TopBuyerResponse>> GetTopBuyersAsync(int top = 5, CancellationToken ct = default);
    Task<IEnumerable<TopSupplierResponse>> GetTopSuppliersAsync(int top = 5, CancellationToken ct = default);
}