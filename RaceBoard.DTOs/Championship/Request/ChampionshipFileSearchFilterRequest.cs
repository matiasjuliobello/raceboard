namespace RaceBoard.DTOs.Championship.Request
{
    public class ChampionshipFileSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdFileType { get; set; }
        public int? IdChampionship { get; set; }
        public int[]? IdsRaceClass { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? UploadTime { get; set; }
    }
}