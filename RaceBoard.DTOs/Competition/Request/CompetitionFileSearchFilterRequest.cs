namespace RaceBoard.DTOs.Competition.Request
{
    public class CompetitionFileSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdFileType { get; set; }
        public int? IdCompetition { get; set; }
        public int[]? IdsRaceClass { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? UploadTime { get; set; }
    }
}