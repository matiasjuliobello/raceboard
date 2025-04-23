namespace RaceBoard.DTOs.Team.Request
{
    public class TeamBoatSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdTeam { get; set; }
        public int? IdChampionship { get; set; }
        public int? IdRaceClass { get; set; }
        public int? IdBoat {  get; set; }
        public string? BoatName { get; set; }
        public string? BoatSailNumber { get; set; }
        public string? BoatHullNumber { get; set; }
    }
}