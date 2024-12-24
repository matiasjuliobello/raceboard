namespace RaceBoard.DTOs.Team.Request
{
    public class TeamMemberSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdTeam { get; set; }
        public int? IdTeamMember { get; set; }
        public int? IdTeamMemberRole { get; set; }
    }
}
