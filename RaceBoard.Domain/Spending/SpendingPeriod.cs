namespace RaceBoard.Domain
{
    public class SpendingPeriod
    {
        public int Id { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}