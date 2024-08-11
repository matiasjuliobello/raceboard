namespace RaceBoard.Domain
{
    public class Project 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ProjectType Type { get; set; }
        public User CreationUser { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
