namespace ProzorroDataMining.Domain.Entities;

using ProzorroDataMining.Domain.Primitives;

public class TenderContract : BaseEntity
{
    public string TenderId { get; private set; } = string.Empty;
    public decimal? ContractValue { get; private set; }
    public string Currency { get; private set; } = "UAH";
    public string? Status { get; private set; }
    public DateTimeOffset? DateSigned { get; private set; }

    public Tender Tender { get; private set; } = null!;

    private TenderContract() { }

    public static TenderContract Create(
        string id,
        string tenderId,
        decimal? contractValue,
        string currency,
        string? status,
        DateTimeOffset? dateSigned) => new()
        {
            Id = id,
            TenderId = tenderId,
            ContractValue = contractValue,
            Currency = currency,
            Status = status,
            DateSigned = dateSigned
        };
}