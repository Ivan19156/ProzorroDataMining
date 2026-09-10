namespace ProzorroDataMining.Contracts.DTOs.Prozorro;

using System.Text.Json.Serialization;

public sealed class TenderListResponse
{
    [JsonPropertyName("data")]
    public List<TenderListItem> Data { get; set; } = new();

    [JsonPropertyName("next_page")]
    public NextPage? NextPage { get; set; }
}

public sealed class TenderListItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("dateModified")]
    public DateTimeOffset DateModified { get; set; }
}

public sealed class NextPage
{
    [JsonPropertyName("offset")]
    public string Offset { get; set; } = string.Empty;

    [JsonPropertyName("uri")]
    public string Uri { get; set; } = string.Empty;
}