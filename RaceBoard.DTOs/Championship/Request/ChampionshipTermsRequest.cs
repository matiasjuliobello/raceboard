namespace RaceBoard.DTOs.Championship.Request
{
    public class ChampionshipTermsRequest
    {
        public int IdChampionship { get; set; }
        public List<ChampionshipTermRequest> Terms { get; set; }
    }
}
