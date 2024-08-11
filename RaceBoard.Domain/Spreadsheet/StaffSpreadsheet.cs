using RaceBoard.Domain.Spreadsheet.Abstract;

namespace RaceBoard.Domain
{
    public class StaffSpreadsheet : PaymentSpreadsheet<StaffSpreadsheet.Item>
    {
        public class Item : PaymentSpreadsheetItem
        {
        }
    }
}
