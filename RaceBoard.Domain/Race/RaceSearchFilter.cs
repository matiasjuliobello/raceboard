namespace RaceBoard.Domain
{
    public class RaceSearchFilter
    {
        public int[] Ids { get; set; }
        public Competition Competition { get; set; }
        public RaceClass RaceClass { get; set; }
    }
}
