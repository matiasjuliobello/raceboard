namespace RaceBoard.DTOs.Race.Request
{
    public class RaceSearchFilterRequest
    {
        public int[] Ids { get; set; }
        public int IdCompetition { get; set; }
        public int IdRaceClass { get; set; }
    }
}