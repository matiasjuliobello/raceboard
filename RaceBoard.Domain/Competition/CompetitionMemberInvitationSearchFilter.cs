namespace RaceBoard.Domain
{
    public class CompetitionMemberInvitationSearchFilter
    {
        public int[]? Ids { get; set; }
        public int? IdCompetition { get; set; }
        public int? IdRequestUser { get; set; }
        public int? IdRole { get; set; }
        public int? IdUser { get; set; }
        public string? Token { get; set; }
        public string? EmailAddress { get; set; }
        public bool? IsPending { get; set; }
        public bool? IsExpired { get; set; }
    }
}