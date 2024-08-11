namespace RaceBoard.DTOs.Spending.Response
{
    public class SpendingResponse
    {
        public int Id { get; set; }
        public int IdEntity { get; set; }
        public DateTimeOffset Date { get; set; }
        public SpendingPeriodResponse Period { get; set; }
        public SpendingItemAmountResponse ItemAmount { get; set; }
    }
}
