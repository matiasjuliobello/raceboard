namespace RaceBoard.Domain
{
    public class OrganizationMemberInvitationSearchFilter
    {
        public int[]? Ids { get; set; }
        public Organization? Organization { get; set; }
        public User? RequestUser { get; set; }
        public Role? Role { get; set; }
        public User? User { get; set; }
        public string? Token { get; set; }
        public string? EmailAddress { get; set; }
        public bool? IsPending { get; set; }
        public bool? IsExpired { get; set; }
    }
}