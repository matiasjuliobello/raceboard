using RaceBoard.DTOs.RaceClass.Request;

namespace RaceBoard.DTOs.Competition.Request
{
    public class CompetitionNotificationRequest
    {
        public int IdCompetition { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public List<RaceClassRequest> RaceClasses { get; set; }
    }
}