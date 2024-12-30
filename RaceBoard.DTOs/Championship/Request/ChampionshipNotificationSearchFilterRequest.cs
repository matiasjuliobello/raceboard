namespace RaceBoard.DTOs.Championship.Request
{
    public class ChampionshipNotificationSearchFilterRequest
    {
        public int[]? Ids { get; set; }
        public int? IdChampionship { get; set; }
        public int[]? IdsRaceClass { get; set; }
    }
}