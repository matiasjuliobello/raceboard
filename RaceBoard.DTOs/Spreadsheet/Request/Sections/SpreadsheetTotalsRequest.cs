namespace RaceBoard.DTOs.Spreadsheet.Request.Sections
{
    public class SpreadsheetTotalsRequest
    {
        public double TotalGeneralRemunerations { get; set; }
        public double TotalHealthInsuranceContribution { get; set; }
        public double TotalDeposit { get; set; }
        public double TotalUnionContribution { get; set; }
        public double NetTotal { get; set; }
    }
}
