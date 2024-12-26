namespace RaceBoard.Domain
{
    public class CompetitionMemberInvitationSearchFilter
    {
        public int[]? Ids { get; set; }
        public Competition? Competition { get; set; }
        public User? RequestUser { get; set; }
        public Role? Role { get; set; }
        public User? User { get; set; }
        public string? Token { get; set; }
        public string? EmailAddress { get; set; }
        public bool? IsPending { get; set; }
        public bool? IsExpired { get; set; }
    }
}