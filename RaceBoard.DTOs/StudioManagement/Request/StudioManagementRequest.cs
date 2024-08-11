namespace RaceBoard.DTOs.StudioManagement.Request
{
    public class StudioManagementRequest
    {
        public string Name { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public string LegalRepresentative { get; set; }
        public string ContactFullName { get; set; }
        public string PhoneNumber { get; set; }
        public string CellNumber { get; set; }
        public string EmailAddress { get; set; }
        public StudioManagementTaxInformationRequest TaxInformation { get; set; }
        public StudioManagementAddressRequest Address { get; set; }
    }
}