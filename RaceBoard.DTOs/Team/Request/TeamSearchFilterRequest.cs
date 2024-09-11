namespace RaceBoard.DTOs.Team.Request
{
    public class TeamSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public string? Name { get; set; }
        public int? IdCompetition { get; set; }
        public int? IdRaceClass { get; set; }
    }
}
