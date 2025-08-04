namespace RaceBoard.Domain
{
    public class CoachOrganizationSearchFilter
    {
        public int[]? Ids { get; set; }
        public Coach? Coach { get; set; }
        public Organization? Organization { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}