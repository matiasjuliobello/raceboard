namespace RaceBoard.Domain
{
    public class RaceSearchFilter
    {
        public int[]? Ids { get; set; }
        public Championship? Championship { get; set; }
        public RaceClass? RaceClass { get; set; }
    }
}
