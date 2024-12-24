namespace RaceBoard.DTOs.Team.Response
{
    public class TeamMemberCheckResponse
    {
        public int Id { get; set; }
        public TeamMemberResponse TeamMember { get; set; }
        public int CheckType { get; set; }
        public DateTimeOffset CheckTime { get; set; }
    }
}