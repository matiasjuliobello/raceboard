namespace RaceBoard.Domain
{
    public class Race
    {
        public int Id { get; set; }
        public List<RaceClass> RaceClasses { get; set; }
        public Championship Championship { get; set; }
        public DateTimeOffset Schedule { get; set; }

        public Race()
        {
            this.RaceClasses = new List<RaceClass>();
        }

    }
}