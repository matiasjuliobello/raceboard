using RaceBoard.DTOs.RaceClass.Response;

namespace RaceBoard.DTOs.Competition.Response
{
    public class CommitteeBoatReturnResponse
    {
        public int Id { get; set; }
        public CompetitionSimpleResponse Competition { get; set; }
        public List<RaceClassResponse> RaceClasses { get; set; }
        public DateTimeOffset ReturnTime { get; set; }
        public string Name { get; set; }
    }
}