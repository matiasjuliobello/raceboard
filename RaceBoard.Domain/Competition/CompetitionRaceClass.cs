namespace RaceBoard.Domain
{
    public class CompetitionRaceClass
    {
        public int Id { get; set; }
        public Competition Competition { get; set; }
        public RaceClass RaceClass { get; set; }
    }
}