namespace RaceBoard.DTOs.Competition.Request
{
    public class CompetitionTermRequest
    {
        public int[] IdsRaceClass { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}