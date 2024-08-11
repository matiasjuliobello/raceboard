namespace RaceBoard.Domain.Spreadsheet.Abstract
{
    public abstract class PaymentSpreadsheet<T> : Spreadsheet
    {
        public Sections.SpreadsheetHeader Header { get; set; }
        public Sections.SpreadsheetProgram Program { get; set; }
        public Sections.SpreadsheetObservations Observations { get; set; }
        public Sections.SpreadsheetTotals Totals { get; set; }

        public int IdPaymentMethod { get; set; }
        public List<T> Items { get; set; }
    }
}
