namespace RaceBoard.Domain.StudioManagement
{
    public class StudioManagement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public string LegalRepresentative { get; set; }
        public string ContactFullName { get; set; }
        public string PhoneNumber { get; set; }
        public string CellNumber { get; set; }
        public string EmailAddress { get; set; }
        public StudioManagementTaxInformation TaxInformation { get; set; }
        public StudioManagementAddress Address { get; set; }
        public StudioManagementImage ImageFile { get; set; }
    }
}