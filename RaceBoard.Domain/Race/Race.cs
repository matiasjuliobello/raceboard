namespace RaceBoard.Domain
{
    public class Race
    {
        public int Id { get; set; }
        public RaceClass Class { get; set; }
        public Competition Competition { get; set; }
    }
}
