namespace RaceBoard.Domain
{
    public class Spending
    {
        public int Id { get; set; }
        public int IdEntity { get; set; }
        public SpendingPeriod Period { get; set; }
        public SpendingItemAmount ItemAmount { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}