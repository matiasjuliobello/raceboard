namespace RaceBoard.DTOs.Competition.Request
{
    public class CommitteeBoatReturnSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdCompetition { get; set; }
        public int[]? IdsRaceClass { get; set; }
        public DateTimeOffset? ReturnTime { get; set; }
    }
}