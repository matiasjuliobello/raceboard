namespace RaceBoard.Domain
{
    public class RaceClass
    {
        public int Id { get; set; }
        public RaceCategory RaceCategory { get; set; }
        public string Name { get; set; }
    }
}
