namespace RaceBoard.Common.Helpers.Interfaces
{
    public interface IDateTimeHelper
    {
        DateTimeOffset GetCurrentTimestamp(DateTimeKind dateTimeKind = DateTimeKind.Utc);
        TimeZoneInfo GetUtcTimeZone();
        TimeZoneInfo GetTimeZone(string id);
        DateTimeOffset ApplyTimeZone(DateTimeOffset dateTimeOffset, TimeZoneInfo timeZone);
        DateTimeOffset ApplyTimeZone(DateTimeOffset dateTimeOffset, string timeZoneIdentifier);
        string GetFormattedTimestamp(DateTimeOffset dateTimeOffset, string timeZoneIdentifier, string format);
    }
}
