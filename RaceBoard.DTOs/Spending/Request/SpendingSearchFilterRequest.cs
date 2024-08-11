namespace RaceBoard.DTOs.Spending.Request
{
    public class SpendingSearchFilterRequest
    {
        public int? Id { get; set; }
        public int? IdPeriod { get; set; }
        public int? IdItem { get; set; }
        public int? IdCategory { get; set; }
        public decimal? Amount {  get; set; }
    }
}
