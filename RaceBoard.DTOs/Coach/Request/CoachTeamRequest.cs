namespace RaceBoard.DTOs.Coach.Request
{
    public class CoachTeamRequest
    {
        public int Id { get; set; }
        public int IdCoach { get; set; }
        public int IdTeam { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}