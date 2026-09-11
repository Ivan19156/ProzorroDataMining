namespace ProzorroDataMining.Domain.Entities;

using ProzorroDataMining.Domain.Primitives;

public class TenderAward : BaseEntity
{
    public string TenderId { get; private set; } = string.Empty;
    public decimal? AwardValue { get; private set; }
    public string? Status { get; private set; }
    public string SupplierName { get; private set; } = string.Empty;
    public string? SupplierId { get; private set; }

    public Tender Tender { get; private set; } = null!;

    private TenderAward() { }

    public static TenderAward Create(
        string id,
        string tenderId,
        decimal? awardValue,
        string? status,
        string supplierName,
        string? supplierId) => new()
        {
            Id = id,
            TenderId = tenderId,
            AwardValue = awardValue,
            Status = status,
            SupplierName = supplierName,
            SupplierId = supplierId
        };
}