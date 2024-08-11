namespace RaceBoard.DTOs.Spending.Response
{
    public class SpendingItemAmountResponse
    {
        public int Id { get; set; }
        public SpendingItemResponse Item { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool IsCurrent { get; set; }
    }
}