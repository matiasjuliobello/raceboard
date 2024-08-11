namespace RaceBoard.DTOs.Spending.Response
{
    public class SpendingPeriodResponse
    {
        public int Id { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public string Description
        {
            get
            {
                return $"{StartDate.Month.ToString().PadLeft(2, '0')}-{StartDate.Year}";
            }
        }
    }
}