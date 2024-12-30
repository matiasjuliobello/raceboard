namespace RaceBoard.DTOs.Team.Request
{
    public class TeamSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdOrganization { get; set; }
        public int? IdChampionship { get; set; }
        public int? IdRaceClass { get; set; }
    }
}
