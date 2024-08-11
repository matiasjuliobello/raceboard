namespace RaceBoard.DTOs.Spreadsheet.Request.Sections
{
    public class SpreadsheetProgramRequest
    {
        public string Author { get; set; }
        public DateTimeOffset PeriodStart { get; set; }
        public DateTimeOffset PeriodEnd { get; set; }
    }
}
