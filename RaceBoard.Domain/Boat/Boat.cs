namespace RaceBoard.Domain
{
    public class Boat : AbstractEntity
    {
        public RaceClass RaceClass { get; set; }
        public string Name { get; set; }
        public string SailNumber { get; set; }
    }
}
