using RaceBoard.DTOs.RaceClass.Response;

namespace RaceBoard.DTOs.Championship.Response
{
    public class ChampionshipTermResponse
    {
        public RaceClassResponse RaceClass { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}