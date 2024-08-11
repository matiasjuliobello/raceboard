namespace RaceBoard.DTOs.StudioManagement.Response
{
    public class StudioManagementResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public string LegalRepresentative { get; set; }
        public string ContactFullName { get; set; }
        public string PhoneNumber { get; set; }
        public string CellNumber { get; set; }
        public string EmailAddress { get; set; }
        public StudioManagementTaxInformationResponse TaxInformation { get; set; }
        public StudioManagementAddressResponse Address { get; set; }
        public StudioManagementImageResponse ImageFile {  get; set; }
    }
}