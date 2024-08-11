namespace RaceBoard.Domain
{
    public class SpendingItemAmount
    {
        public int Id { get; set; }
        public SpendingItem Item { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool IsCurrent { get; set; }
    }
}