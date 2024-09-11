namespace RaceBoard.Domain
{
    public class BoatSearchFilter
    {
        public int[]? Ids { get; set; }
        public RaceClass? RaceClass { get; set; }
        public RaceCategory? RaceCategory { get; set; }
        public string? Name { get; set; }
        public string? SailNumber { get; set; }
    }
}