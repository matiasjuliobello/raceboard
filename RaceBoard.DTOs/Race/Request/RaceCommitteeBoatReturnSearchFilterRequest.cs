namespace RaceBoard.DTOs.Race.Request
{
    public class RaceCommitteeBoatReturnSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdCompetition { get; set; }
        public DateTimeOffset? Return { get; set; }
    }
}