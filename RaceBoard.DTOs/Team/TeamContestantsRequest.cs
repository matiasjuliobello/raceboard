namespace RaceBoard.DTOs.Team
{
    public class TeamContestantsRequest
    {
        public int IdTeam { get; set; }
        public List<TeamContestantRequest> Contestants { get; set; }
    }
}
