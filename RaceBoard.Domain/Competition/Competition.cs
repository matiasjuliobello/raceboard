namespace RaceBoard.Domain
{
    public class Competition : AbstractEntity
    {
        public string Name { get; set; }
        public City City { get; set; }
        public List<Organization> Organizations { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
