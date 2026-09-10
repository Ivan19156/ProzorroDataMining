// Domain/Entities/Tender.cs
namespace ProzorroDataMining.Domain.Entities;

using ProzorroDataMining.Domain.Primitives;

public class Tender : AggregateRoot
{
    public string Status { get; private set; } = string.Empty;
    public decimal BudgetAmount { get; private set; }
    public string BudgetCurrency { get; private set; } = "UAH";
    public string? ProcuringEntityId { get; private set; }
    public string ProcuringEntityName { get; private set; } = string.Empty;
    public DateTimeOffset DateCreated { get; private set; }
    public DateTimeOffset DateModified { get; private set; }
    public DateTimeOffset FetchedAt { get; private set; }

    private readonly List<TenderItem> _items = new();
    private readonly List<TenderContract> _contracts = new();
    private readonly List<TenderAward> _awards = new();

    public IReadOnlyCollection<TenderItem> Items => _items.AsReadOnly();
    public IReadOnlyCollection<TenderContract> Contracts => _contracts.AsReadOnly();
    public IReadOnlyCollection<TenderAward> Awards => _awards.AsReadOnly();

    private Tender() { }

    public static Tender Create(
        string id,
        string status,
        decimal budgetAmount,
        string budgetCurrency,
        string? procuringEntityId,
        string procuringEntityName,
        DateTimeOffset dateCreated,
        DateTimeOffset dateModified) => new()
        {
            Id = id,
            Status = status,
            BudgetAmount = budgetAmount,
            BudgetCurrency = budgetCurrency,
            ProcuringEntityId = procuringEntityId,
            ProcuringEntityName = procuringEntityName,
            DateCreated = dateCreated,
            DateModified = dateModified,
            FetchedAt = DateTimeOffset.UtcNow
        };

    public void AddItem(TenderItem item) => _items.Add(item);
    public void AddContract(TenderContract contract) => _contracts.Add(contract);
    public void AddAward(TenderAward award) => _awards.Add(award);
}