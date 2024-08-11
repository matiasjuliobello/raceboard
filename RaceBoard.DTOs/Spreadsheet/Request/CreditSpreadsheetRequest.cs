using RaceBoard.DTOs.Spreadsheet.Request.Abstract;

namespace RaceBoard.DTOs.Spreadsheet.Request
{
    public class CreditSpreadsheetRequest : SpreadsheetRequest
    {
        public List<CreditSpreadsheetItemRequest> Items { get; set; }
    }
}
