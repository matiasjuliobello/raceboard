namespace RaceBoard.Domain
{
    public class RaceClass : AbstractEntity
    {
        public RaceCategory RaceCategory { get; set; }
        public string Name { get; set; }
    }
}
