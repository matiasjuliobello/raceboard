namespace RaceBoard.Domain
{
    public class SpendingSearchFilter
    {
        public int? Id { get; set; }
        public SpendingPeriod Period { get; set; }
        public SpendingItem Item { get; set; }
        public SpendingItemCategory Category { get; set; }
        public decimal? Amount { get; set; }
    }
}