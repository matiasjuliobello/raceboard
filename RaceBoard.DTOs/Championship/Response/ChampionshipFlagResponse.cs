using RaceBoard.DTOs.Flag.Response;
using RaceBoard.DTOs.Person.Response;

namespace RaceBoard.DTOs.Championship.Response
{
    public class ChampionshipFlagResponse
    {
        public int Id { get; set; }
        public FlagResponse Flag { get; set; }
        public PersonSimpleResponse Person { get; set; }
        public DateTimeOffset Raising { get; set; }
        public DateTimeOffset? Lowering { get; set; }
        public int Order { get; set; }
    }
}
