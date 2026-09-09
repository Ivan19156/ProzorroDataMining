using System.Text;

namespace ProzorroDataMining.Shared.Constants;

public static class ProzorroConstants
{
    public const string BaseUrl = "https://public-api.prozorro.gov.ua/";
    public const string TargetCpvCode = "09310000-5";
    public const string TargetStatus = "complete";

    public static class Api
    {
        public const string TenderList = "api/2.5/tenders?descending=1";
        private static readonly CompositeFormat TenderDetailFormat =
               CompositeFormat.Parse("api/2.5/tenders/{0}");

        public static string TenderDetail(string id) =>
               string.Format(null, TenderDetailFormat, id);

        public const string TenderListFiltered =
        "api/2.5/tenders?descending=1" +
        "&dateModified.lt=2026-01-01T00%3A00%3A00Z" +
        "&dateModified.gte=2025-12-01T00%3A00%3A00Z";
    }

    public static class Filters
    {
        public static readonly DateTimeOffset PeriodStart =
            new(2025, 12, 1, 0, 0, 0, TimeSpan.FromHours(2));
        public static readonly DateTimeOffset PeriodEnd =
            new(2026, 1, 1, 0, 0, 0, TimeSpan.FromHours(2));
    }
}