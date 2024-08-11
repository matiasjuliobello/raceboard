using RaceBoard.Domain.Spreadsheet.Abstract;

namespace RaceBoard.Domain
{
    public class ActorSpreadsheet : PaymentSpreadsheet<ActorSpreadsheet.Item>
    {
        public class Item : PaymentSpreadsheetItem
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
}