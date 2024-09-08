namespace RaceBoard.Domain
{
    public class Organization : AbstractEntity
    {
        public string Name { get; set; }
        public City City { get; set; }
    }
}
