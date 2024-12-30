using RaceBoard.DTOs.RaceClass.Request;

namespace RaceBoard.DTOs.Championship.Request
{
    public class ChampionshipNotificationRequest
    {
        public int IdChampionship { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public List<RaceClassRequest> RaceClasses { get; set; }
    }
}