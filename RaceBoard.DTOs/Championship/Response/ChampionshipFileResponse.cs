using RaceBoard.DTOs.File.Response;
using RaceBoard.DTOs.RaceClass.Response;

namespace RaceBoard.DTOs.Championship.Response
{
    public class ChampionshipFileResponse
    {
        public int Id {  get; set; }
        public ChampionshipSimpleResponse Championship { get; set; }
        public FileResponse File { get; set; }
        public FileTypeResponse FileType { get; set; }
        public List<RaceClassResponse> RaceClasses { get; set; }
    }
}