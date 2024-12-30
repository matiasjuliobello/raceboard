namespace RaceBoard.Domain
{
    public class TeamSearchFilter
    {
        public int[]? Ids { get; set; }
        public Organization? Organization { get; set; }
        public Championship? Championship { get; set; }
        public RaceClass? RaceClass { get; set; }
    }
}