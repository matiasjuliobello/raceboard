namespace RaceBoard.Domain
{
    public class OrganizationMemberSearchFilter
    {
        public int[]? Ids { get; set; }
        public int? IdOrganization { get; set; }
        public int? IdRole { get; set; }
        public int? IdUser { get; set; }
    }
}