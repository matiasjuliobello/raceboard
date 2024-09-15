using RaceBoard.DTOs.RaceClass.Response;

namespace RaceBoard.DTOs.Competition.Response
{
    public class CompetitionTermResponse
    {
        public RaceClassResponse RaceClass { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}