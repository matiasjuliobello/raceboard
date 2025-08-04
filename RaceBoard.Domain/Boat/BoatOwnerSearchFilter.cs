namespace RaceBoard.Domain
{
    public class BoatOwnerSearchFilter
    {
        public int[]? Ids { get; set; }
        public Boat? Boat { get; set; }
        public Person? Person { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}