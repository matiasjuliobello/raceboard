namespace RaceBoard.Domain
{
    public class ChampionshipNotificationSearchFilter
    {
        public int[]? Ids { get; set; }
        public Championship? Championship { get; set; }
        public List<RaceClass>? RaceClasses { get; set; }
    }
}