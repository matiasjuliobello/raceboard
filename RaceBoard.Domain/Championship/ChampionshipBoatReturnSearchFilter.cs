namespace RaceBoard.Domain
{
    public class ChampionshipBoatReturnSearchFilter
    {
        public int[]? Ids { get; set; }
        public Championship? Championship { get; set; }
        public List<RaceClass>? RaceClasses { get; set; }
        public DateTimeOffset? ReturnTime { get; set; }
    }
}
