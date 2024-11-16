namespace RaceBoard.DTOs.Team.Request
{
    public class TeamCheckRequest
    {
        public int Id { get; set; }
        public int IdTeamContestant { get; set; }
        public int IdCheckType { get; set; }
        public DateTimeOffset CheckTime { get; set; }
    }
}
