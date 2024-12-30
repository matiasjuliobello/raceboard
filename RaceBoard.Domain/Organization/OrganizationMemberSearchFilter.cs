namespace RaceBoard.Domain
{
    public class OrganizationMemberSearchFilter
    {
        public int[]? Ids { get; set; }
        public Organization? Organization { get; set; }
        public Role? Role { get; set; }
        public User? User { get; set; }
        public bool? IsActive {  get; set; }
    }
}