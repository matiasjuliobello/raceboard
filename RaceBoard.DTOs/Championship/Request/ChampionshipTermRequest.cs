namespace RaceBoard.DTOs.Championship.Request
{
    public class ChampionshipTermRequest
    {
        public int[] IdsRaceClass { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}