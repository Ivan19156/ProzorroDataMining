
namespace ProzorroDataMining.Domain.Entities;

using ProzorroDataMining.Domain.Primitives;

public class TenderItem : BaseEntity
{
    public string TenderId { get; private set; } = string.Empty;
    public string CpvCode { get; private set; } = string.Empty;
    public string? CpvDescription { get; private set; }
    public string? Description { get; private set; }
    public decimal? Quantity { get; private set; }
    public string? UnitName { get; private set; }
    public string? UnitCode { get; private set; }

    public Tender Tender { get; private set; } = null!;

    private TenderItem() { }

    public static TenderItem Create(
        string id,
        string tenderId,
        string cpvCode,
        string? cpvDescription,
        string? description,
        decimal? quantity,
        string? unitName,
        string? unitCode) => new()
        {
            Id = id,
            TenderId = tenderId,
            CpvCode = cpvCode,
            CpvDescription = cpvDescription,
            Description = description,
            Quantity = quantity,
            UnitName = unitName,
            UnitCode = unitCode
        };
}