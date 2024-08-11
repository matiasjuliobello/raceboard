namespace RaceBoard.DTOs.Spreadsheet.Request.Abstract
{
    public abstract class PaymentSpreadsheetItemRequest
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
