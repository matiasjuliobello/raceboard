namespace RaceBoard.Domain
{
    public class Race : AbstractEntity
    {
        public int Id { get; set; }
        public RaceClass RaceClass { get; set; }
        public Competition Competition { get; set; }
    }
}
