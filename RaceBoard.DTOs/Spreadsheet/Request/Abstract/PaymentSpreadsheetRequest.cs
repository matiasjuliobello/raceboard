using Sections = RaceBoard.DTOs.Spreadsheet.Request.Sections;

namespace RaceBoard.DTOs.Spreadsheet.Request.Abstract
{
    public abstract class PaymentSpreadsheetRequest<T> : SpreadsheetRequest
    {
        public int IdPaymentMethod { get; set; }
        public Sections.SpreadsheetHeaderRequest Header { get; set; }
        public Sections.SpreadsheetProgramRequest Program { get; set; }
        public Sections.SpreadsheetObservationsRequest Observations { get; set; }
        public Sections.SpreadsheetTotalsRequest Totals { get; set; }
        public List<T> Items { get; set; }
    }
}