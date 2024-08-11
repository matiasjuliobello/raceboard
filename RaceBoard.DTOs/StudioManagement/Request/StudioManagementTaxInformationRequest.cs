namespace RaceBoard.DTOs.StudioManagement.Request
{
    public class StudioManagementTaxInformationRequest
    {
        public string CompanyName { get; set; }
        public string TaxIdentificationNumber { get; set; }
        public string GrossIncomeTax { get; set; }
    }
}