namespace RaceBoard.DTOs.Competition.Request
{
    public class CompetitionNotificationSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdCompetition { get; set; }
        public int[]? IdsRaceClass { get; set; }
    }
}