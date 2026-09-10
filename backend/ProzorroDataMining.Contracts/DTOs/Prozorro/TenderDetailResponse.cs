namespace ProzorroDataMining.Contracts.DTOs.Prozorro;

using System.Text.Json.Serialization;

public sealed class TenderDetailResponse
{
    [JsonPropertyName("data")]
    public TenderData Data { get; set; } = null!;
}

public sealed class TenderData
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("dateCreated")]
    public DateTimeOffset DateCreated { get; set; }

    [JsonPropertyName("dateModified")]
    public DateTimeOffset DateModified { get; set; }

    [JsonPropertyName("value")]
    public MoneyValue? Value { get; set; }

    [JsonPropertyName("procuringEntity")]
    public ProcuringEntity? ProcuringEntity { get; set; }

    [JsonPropertyName("items")]
    public List<TenderItemData>? Items { get; set; }

    [JsonPropertyName("contracts")]
    public List<ContractData>? Contracts { get; set; }

    [JsonPropertyName("awards")]
    public List<AwardData>? Awards { get; set; }
}

public sealed class MoneyValue
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "UAH";
}

public sealed class ProcuringEntity
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("identifier")]
    public Identifier? Identifier { get; set; }
}

public sealed class Identifier
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}

public sealed class TenderItemData
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("classification")]
    public Classification? Classification { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("quantity")]
    public decimal? Quantity { get; set; }

    [JsonPropertyName("unit")]
    public Unit? Unit { get; set; }
}

public sealed class Classification
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

public sealed class Unit
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }
}

public sealed class ContractData
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public MoneyValue? Value { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("dateSigned")]
    public DateTimeOffset? DateSigned { get; set; }
}

public sealed class AwardData
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public MoneyValue? Value { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("suppliers")]
    public List<Supplier>? Suppliers { get; set; }
}

public sealed class Supplier
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("identifier")]
    public Identifier? Identifier { get; set; }
}