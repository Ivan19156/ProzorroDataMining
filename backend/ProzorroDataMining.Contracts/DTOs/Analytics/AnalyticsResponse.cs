namespace ProzorroDataMining.Contracts.Analytics;

public sealed class BudgetSavingsResponse
{
    public decimal TotalBudget { get; init; }
    public decimal TotalContracts { get; init; }
    public decimal TotalSavings { get; init; }
}

public sealed class TopBuyerResponse
{
    public string Name { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public int ContractCount { get; init; }
}

public sealed class TopSupplierResponse
{
    public string Name { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public int AwardCount { get; init; }
}