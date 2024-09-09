using RaceBoard.DTOs.Competition.Response;
using RaceBoard.DTOs.RaceClass.Response;

namespace RaceBoard.DTOs.Team.Response
{
    public class TeamResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CompetitionResponse Competition { get; set; }
        public RaceClassResponse RaceClass { get; set; }
    }
}
