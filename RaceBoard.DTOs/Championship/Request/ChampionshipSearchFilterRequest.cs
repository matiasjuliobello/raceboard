namespace RaceBoard.DTOs.Championship.Request
{
    public class ChampionshipSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public string? Name { get; set; }
        public int? IdCity { get; set; }
        public int[]? IdsOrganization { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}