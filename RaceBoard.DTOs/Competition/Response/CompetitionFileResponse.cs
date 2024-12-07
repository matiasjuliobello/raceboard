using RaceBoard.DTOs.File.Response;
using RaceBoard.DTOs.RaceClass.Response;

namespace RaceBoard.DTOs.Competition.Response
{
    public class CompetitionFileResponse
    {
        public int Id {  get; set; }
        public CompetitionSimpleResponse Competition { get; set; }
        public FileResponse File { get; set; }
        public FileTypeResponse FileType { get; set; }
        public List<RaceClassResponse> RaceClasses { get; set; }
    }
}