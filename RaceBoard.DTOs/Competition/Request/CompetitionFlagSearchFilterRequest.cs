namespace RaceBoard.DTOs.Competition.Request
{
    public class CompetitionFlagSearchFilterRequest
    {
        public int? IdFlag { get; set; }
        public int? IdPerson { get; set; }
        public DateTimeOffset? Raising { get; set; }
        public DateTimeOffset? Lowering { get; set; }
    }
}
