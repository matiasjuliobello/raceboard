namespace RaceBoard.Domain
{
    public class Race
    {
        public int Id { get; set; }
        public RaceClass RaceClass { get; set; }
        public Competition Competition { get; set; }
    }
}
