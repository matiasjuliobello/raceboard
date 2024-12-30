namespace RaceBoard.DTOs.Team.Request
{
    public class TeamCheckSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdChampionship { get; set; }
        public int[]? IdsRaceClass { get; set; }
        public int? IdTeam { get; set; }
        public int? IdCheckType { get; set; }
        public DateTimeOffset? DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }
    }
}