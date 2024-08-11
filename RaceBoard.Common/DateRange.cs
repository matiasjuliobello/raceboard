namespace RaceBoard.Common
{
    public class DateRange
    {
        public DateTimeOffset DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }

        public DateRange(DateTimeOffset dateFrom, DateTimeOffset? dateTo)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
        }
    }
}
