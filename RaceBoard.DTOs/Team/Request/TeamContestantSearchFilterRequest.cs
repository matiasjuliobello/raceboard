namespace RaceBoard.DTOs.Team.Request
{
    public class TeamContestantSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdTeam { get; set; }
        public int? IdContestant { get; set; }
        public int? IdContestantRole { get; set; }
    }
}
