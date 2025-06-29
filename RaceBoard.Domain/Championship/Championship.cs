namespace RaceBoard.Domain
{
    public class Championship
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public City City { get; set; }
        public List<Organization> Organizations { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public File? ImageFile { get; set; }
        public User CreationUser { get; set; }
        //public List<ChampionshipGroup> Groups { get; set; }

        public Championship()
        {
            this.Organizations = new List<Organization>();
        }
    }
}