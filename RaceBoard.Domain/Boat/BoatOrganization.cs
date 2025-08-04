namespace RaceBoard.Domain
{
    public class BoatOrganization
    {
        public int Id { get; set; }
        public Boat Boat { get; set; }
        public Organization Organization { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
