using RaceBoard.DTOs.RaceClass.Request;

namespace RaceBoard.DTOs.Championship.Request
{
    public class ChampionshipFileRequest
    {
        public int IdChampionship { get; set; }
        public int IdFileType { get; set; }
        public string Description { get; set; }
        public List<RaceClassRequest> RaceClasses { get; set; }
    }
}