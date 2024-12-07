using RaceBoard.DTOs.RaceClass.Request;

namespace RaceBoard.DTOs.Competition.Request
{
    public class CompetitionFileUploadRequest
    {
        public int IdCompetition { get; set; }
        public int IdFileType { get; set; }
        public string Description { get; set; }
        public List<RaceClassRequest> RaceClasses { get; set; }
    }
}