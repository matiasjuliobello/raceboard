namespace RaceBoard.DTOs.Competition.Request
{
    public class CompetitionRaceClassRequest
    {
        public int IdCompetition { get; set; }
        public int[] IdsRaceClass { get; set; }
    }
}