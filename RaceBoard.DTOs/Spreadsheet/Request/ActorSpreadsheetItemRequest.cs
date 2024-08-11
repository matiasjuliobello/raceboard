using RaceBoard.DTOs.Spreadsheet.Request.Abstract;

namespace RaceBoard.DTOs.Spreadsheet.Request
{
    public class ActorSpreadsheetItemRequest : PaymentSpreadsheetItemRequest
    {
        public double Bolus { get; set; }
        public double Outdoors { get; set; }
        public double Surpluses { get; set; }
        public double Reshoots { get; set; }
        public double Other { get; set; }
        public double TotalGrossRemuneration { get; set; }
        public double PerDiem { get; set; }
        public double Voucher { get; set; }
    }
}
