namespace RaceBoard.Domain
{
    public class TeamBoatSearchFilter
    {
        public int[]? Ids { get; set; }
        public Boat? Boat { get; set; }
        public Team? Team { get; set; }
    }
}