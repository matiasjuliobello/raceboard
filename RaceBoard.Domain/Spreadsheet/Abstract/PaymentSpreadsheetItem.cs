namespace RaceBoard.Domain.Spreadsheet.Abstract
{
    public abstract class PaymentSpreadsheetItem
    {
        public int IdPayment { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string Provider { get; set; }
        public string ProviderRole { get; set; }
        public DateTimeOffset CollaborationDate { get; set; }
        public int EpisodeNumber { get; set; }
        public double Contract { get; set; }
    }
}
