namespace RaceBoard.Domain
{
    public class TeamMemberInvitationSearchFilter
    {
        public int[]? Ids { get; set; }
        public Team? Team { get; set; }
        public User? RequestUser { get; set; }
        public TeamMemberRole? Role { get; set; }
        public User? User { get; set; }
        public string? Token { get; set; }
        public string? EmailAddress { get; set; }
        public bool? IsPending { get; set; }
        public bool? IsExpired { get; set; }
    }
}