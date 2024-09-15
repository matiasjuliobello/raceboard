namespace RaceBoard.DTOs.Competition.Request
{
    public class CompetitionNewsUpdateRequest
    {
        public int IdCompetition { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}