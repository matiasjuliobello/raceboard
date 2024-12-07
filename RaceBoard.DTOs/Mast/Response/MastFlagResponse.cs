using RaceBoard.DTOs.Flag.Response;
using RaceBoard.DTOs.Person.Response;

namespace RaceBoard.DTOs.Mast.Response
{
    public class MastFlagResponse
    {
        public int Id { get; set; }
        public MastResponse Mast { get; set; }
        public FlagResponse Flag { get; set; }
        public PersonSimpleResponse Person { get; set; }
        public DateTimeOffset RaisingMoment { get; set; }
        public DateTimeOffset? LoweringMoment { get; set; }
        public bool IsActive { get; set; }
    }
}
