using RaceBoard.DTOs.RaceClass.Response;

namespace RaceBoard.DTOs.Championship.Response
{
    public class ChampionshipBoatReturnResponse
    {
        public int Id { get; set; }
        public ChampionshipSimpleResponse Championship { get; set; }
        public List<RaceClassResponse> RaceClasses { get; set; }
        public DateTimeOffset ReturnTime { get; set; }
        public string Name { get; set; }
    }
}