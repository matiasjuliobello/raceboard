namespace RaceBoard.Domain
{
    public class TeamSearchFilter
    {
        public int[]? Ids { get; set; }
        public string? Name { get; set; }
        public Organization? Organization { get; set; }
        public Competition? Competition { get; set; }
        public RaceClass? RaceClass { get; set; }
    }
}