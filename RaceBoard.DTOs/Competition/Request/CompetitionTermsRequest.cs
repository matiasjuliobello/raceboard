namespace RaceBoard.DTOs.Competition.Request
{
    public class CompetitionTermsRequest
    {
        public int IdCompetition { get; set; }
        public List<CompetitionTermRequest> Terms { get; set; }
    }
}
