using RaceBoard.DTOs.Competition.Response;
using RaceBoard.DTOs.RaceClass.Response;

namespace RaceBoard.DTOs.Race.Response
{
    public class RaceResponse
    {
        public int Id { get; set; }
        public CompetitionResponse Competition { get; set; }
        public RaceClassResponse RaceClass { get; set; }
        public DateTimeOffset Schedule { get; set; }
    }
}
