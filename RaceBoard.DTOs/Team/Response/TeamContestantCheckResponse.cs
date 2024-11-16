namespace RaceBoard.DTOs.Team.Response
{
    public class TeamContestantCheckResponse
    {
        public int Id { get; set; }
        public TeamContestantResponse TeamContestant { get; set; }
        public int CheckType { get; set; }
        public DateTimeOffset CheckTime { get; set; }
    }
}