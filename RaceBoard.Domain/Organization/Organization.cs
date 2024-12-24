namespace RaceBoard.Domain
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public City City { get; set; }
        public User CreationUser {  get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
