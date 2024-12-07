using RaceBoard.DTOs.RaceClass.Response;

namespace RaceBoard.DTOs.Competition.Response
{
    public class CompetitionNotificationResponse
    {
        public string Message { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public List<RaceClassResponse> RaceClasses { get; set; }
    }
}
