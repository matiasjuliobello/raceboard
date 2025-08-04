namespace RaceBoard.Domain
{
    public class CoachOrganization
    {
        public int Id { get; set; }
        public Coach Coach { get; set; }
        public Organization Organization { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}