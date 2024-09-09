namespace RaceBoard.DTOs.Organization.Request
{
    public class OrganizationSearchFilterRequest
    {
        public int[] Ids { get; set; }
        public string Name { get; set; }
        public int IdCity { get; set; }
    }
}
