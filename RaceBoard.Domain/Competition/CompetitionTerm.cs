namespace RaceBoard.Domain
{
    public abstract class CompetitionTerm
    {
        public int Id { get; set; }
        public Competition Competition { get; set; }
        public RaceClass RaceClass { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}