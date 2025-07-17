using RaceBoard.DTOs.RaceClass.Request;

namespace RaceBoard.DTOs.Championship.Request
{
    public class ChampionshipFileRequest
    {
        public int IdChampionship { get; set; }
        public int IdFileType { get; set; }
        public List<RaceClassRequest> RaceClasses { get; set; }
        public string Name { get; set; }
    }
}