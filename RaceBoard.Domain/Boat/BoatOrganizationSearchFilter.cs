namespace RaceBoard.Domain
{
    public class BoatOrganizationSearchFilter
    {
        public int[]? Ids { get; set; }
        public Boat? Boat { get; set; }
        public Organization? Organization { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}