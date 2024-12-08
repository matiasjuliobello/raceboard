namespace RaceBoard.Domain
{
    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public City City { get; set; }
        public List<Organization> Organizations { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public File? ImageFile {  get; set; }
    }
}