using RaceBoard.DTOs.Competition.Response;

namespace RaceBoard.DTOs.Mast.Response
{
    public class MastResponse
    {
        public int Id { get; set; }
        public CompetitionSimpleResponse Competition { get; set; }
    }
}