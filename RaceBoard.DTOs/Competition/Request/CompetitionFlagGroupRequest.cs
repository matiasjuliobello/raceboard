namespace RaceBoard.DTOs.Competition.Request
{
    public class CompetitionFlagGroupRequest
    {
        public int Id { get; set; }
        public int IdCompetition { get; set; }
        public CompetitionFlagRequest[] Flags { get; set; }
    }
}
